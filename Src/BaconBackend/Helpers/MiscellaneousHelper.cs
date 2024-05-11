﻿using BaconBackend.DataObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace BaconBackend.Helpers
{
    public enum RedditContentType
    {
        Subreddit,
        Post,
        Comment
    }

    public class RedditContentContainer
    {
        public RedditContentType Type;
        public string Subreddit;
        public string Post;
        public string Comment;
    }

    public class MiscellaneousHelper
    {
        /// <summary>
        /// Called when the user is trying to comment on something.
        /// </summary>
        /// <returns>Returns the json returned or a null string if failed.</returns>
        public static async Task<string> SendRedditComment(BaconManager baconMan, string redditIdCommentingOn, string comment)
        {
            string returnString = null;
            try
            {
                // Build the data to send
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("thing_id", redditIdCommentingOn));
                postData.Add(new KeyValuePair<string, string>("text", comment));

                // Make the call
                returnString = await baconMan.NetworkMan.MakeRedditPostRequest("api/comment", postData);
            }
            catch (Exception e)
            {
                baconMan.TelemetryMan.ReportUnExpectedEvent("MisHelper", "failed to send comment", e);
                baconMan.MessageMan.DebugDia("failed to send message", e);
            }
            return returnString;
        }

        /// <summary>
        /// Gets a reddit user.
        /// </summary>
        /// <returns>Returns null if it fails or the user doesn't exist.</returns>
        public static async Task<User> GetRedditUser(BaconManager baconMan, string userName)
        {
            User foundUser = null;
            try
            {
                // Make the call
                string jsonResponse = await baconMan.NetworkMan.MakeRedditGetRequest($"user/{userName}/about/.json");

                // Try to parse out the user
                int dataPos = jsonResponse.IndexOf("\"data\":");
                if (dataPos == -1) return null;
                int dataStartPos = jsonResponse.IndexOf('{', dataPos + 7);
                if (dataPos == -1) return null;
                int dataEndPos = jsonResponse.IndexOf("}", dataStartPos);
                if (dataPos == -1) return null;

                string userData = jsonResponse.Substring(dataStartPos, (dataEndPos - dataStartPos + 1));

                // Parse the new user
                foundUser = await Task.Run(() => JsonConvert.DeserializeObject<User>(userData));
            }
            catch (Exception e)
            {
                baconMan.TelemetryMan.ReportUnExpectedEvent("MisHelper", "failed to search for user", e);
                baconMan.MessageMan.DebugDia("failed to search for user", e);
            }
            return foundUser;
        }

        /// <summary>
        /// Saves, unsaves, hides, or unhides a reddit item.
        /// </summary>
        /// <returns>Returns null if it fails or the user doesn't exist.</returns>
        public static async Task<bool> SaveOrHideRedditItem(BaconManager baconMan, string redditId, bool? save, bool? hide)
        {
            if(!baconMan.UserMan.IsUserSignedIn)
            {
                baconMan.MessageMan.ShowSigninMessage(save.HasValue ? "save item" : "hide item");
                return false;
            }

            bool wasSuccess = false;
            try
            {
                // Make the data
                List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
                data.Add(new KeyValuePair<string, string>("id", redditId));

                string url;
                if (save.HasValue)
                {
                    url = save.Value ? "/api/save" : "/api/unsave";
                }
                else if(hide.HasValue)
                {
                    url = hide.Value ? "/api/hide" : "/api/unhide";
                }
                else
                {
                    return false;
                }

                // Make the call
                string jsonResponse = await baconMan.NetworkMan.MakeRedditPostRequest(url, data);

                if(jsonResponse.Contains("{}"))
                {
                    wasSuccess = true;
                }
                else
                {
                    baconMan.TelemetryMan.ReportUnExpectedEvent("MisHelper", "failed to save or hide item, unknown response");
                    baconMan.MessageMan.DebugDia("failed to save or hide item, unknown response");
                }
            }
            catch (Exception e)
            {
                baconMan.TelemetryMan.ReportUnExpectedEvent("MisHelper", "failed to save or hide item", e);
                baconMan.MessageMan.DebugDia("failed to save or hide item", e);
            }
            return wasSuccess;
        }


        /// <summary>
        /// Attempts to parse out a reddit object from a reddit data object.
        /// </summary>
        /// <param name="orgionalJson"></param>
        /// <returns></returns>
        public static string ParseOutRedditDataElement(string orgionalJson)
        {
            try
            {
                // Try to parse out the object
                int dataPos = orgionalJson.IndexOf("\"data\":");
                if (dataPos == -1) return null;
                int dataStartPos = orgionalJson.IndexOf('{', dataPos + 7);
                if (dataPos == -1) return null;
                int dataEndPos = orgionalJson.IndexOf("}", dataStartPos);
                if (dataPos == -1) return null;

                return orgionalJson.Substring(dataStartPos, (dataEndPos - dataStartPos + 1));
            }
            catch(Exception)
            {

            }

            return null;
        }

        /// <summary>
        /// Attempts to find some reddit content in a link. A subreddit, post or comments.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RedditContentContainer TryToFindRedditContentInLink(string url)
        {
            string urlLower = url.ToLower();
            RedditContentContainer containter = null;

            // Try to find /r/ or r/ links
            if (urlLower.StartsWith("/r/") || urlLower.StartsWith("r/"))
            {
                // Get the display name
                int subStart = urlLower.IndexOf("r/");
                subStart += 2;

                // Make sure we don't have a trailing slash.
                int subEnd = urlLower.Length;
                if (urlLower.Length > 0 && urlLower[urlLower.Length - 1] == '/')
                {
                    subEnd--;
                }

                // Get the name.
                string displayName = urlLower.Substring(subStart, subEnd - subStart).Trim();
                containter = new RedditContentContainer()
                {
                    Type = RedditContentType.Subreddit,
                    Subreddit = displayName
                };
            }
            // Try to find any other reddit link
            else if(urlLower.Contains("reddit.com/"))
            {
                // Try to find the start of the subreddit
                int startSub = urlLower.IndexOf("r/");
                if(startSub != -1)
                {
                    startSub += 2;

                    // Try to find the end of the subreddit.
                    int endSub = FindNextUrlBreak(urlLower, startSub);

                    if (endSub > startSub)
                    {
                        // We found a subreddit!
                        containter = new RedditContentContainer();
                        containter.Subreddit = urlLower.Substring(startSub, endSub - startSub);
                        containter.Type = RedditContentType.Subreddit;

                        // See if we have a post
                        int postStart = url.IndexOf("comments/");
                        if(postStart != -1)
                        {
                            postStart += 9;

                            // Try to find the end
                            int postEnd = FindNextUrlBreak(urlLower, postStart);

                            if(postEnd > postStart)
                            {
                                // We found a post! Build on top of the subreddit
                                containter.Post = urlLower.Substring(postStart, postEnd - postStart);
                                containter.Type = RedditContentType.Post;

                                // Try to find a comment, for there to be a comment this should have a / after it.
                                if(urlLower.Length > postEnd && urlLower[postEnd] == '/')
                                {
                                    postEnd++;
                                    // Now try to find the / after the post title
                                    int commentStart = urlLower.IndexOf('/', postEnd);
                                    if(commentStart != -1)
                                    {
                                        commentStart++;

                                        // Try to find the end of the comment
                                        int commentEnd = FindNextUrlBreak(urlLower, commentStart);

                                        if(commentEnd > commentStart )
                                        {
                                            // We found a comment!
                                            containter.Comment = urlLower.Substring(commentStart, commentEnd - commentStart);
                                            containter.Type = RedditContentType.Comment;
                                        }
                                    }
                                }                               
                            }
                        }
                    }
                }
            }

            return containter;
        }

        private static int FindNextUrlBreak(string url, int startingPos)
        {
            int nextBreak = startingPos;
            while (url.Length > nextBreak && (Char.IsLetterOrDigit(url[nextBreak]) || url[nextBreak] == '_'))
            {
                nextBreak++;
            }
            return nextBreak;
        }

        public static Color GetComplementaryColor(Color source)
        {
            Color inputColor = source;
            // If RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, 
            // decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker
            // side, decrease yellow by about 50% (it will increase in conversion)
            byte avgColorValue = (byte)((source.R + source.G + source.B) / 3);
            int diff_r = Math.Abs(source.R - avgColorValue);
            int diff_g = Math.Abs(source.G - avgColorValue);
            int diff_b = Math.Abs(source.B - avgColorValue);
            if (diff_r < 20 && diff_g < 20 && diff_b < 20) //The color is a shade of gray
            {
                if (avgColorValue < 123) //color is dark
                {
                    inputColor.B = 220;
                    inputColor.G = 230;
                    inputColor.R = 50;
                }
                else
                {
                    inputColor.R = 255;
                    inputColor.G = 255;
                    inputColor.B = 50;
                }
            }

            RGB rgb = new RGB { R = inputColor.R, G = inputColor.G, B = inputColor.B };
            HSB hsb = ConvertToHSB(rgb);
            hsb.H = hsb.H < 180 ? hsb.H + 180 : hsb.H - 180;
            //hsb.B = isColorDark ? 240 : 50; //Added to create dark on light, and light on dark
            rgb = ConvertToRGB(hsb);
            return new Color { A = 255, R = (byte)rgb.R, G = (byte)rgb.G, B = (byte)rgb.B };
        }

        internal static RGB ConvertToRGB(HSB hsb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            double chroma = hsb.S * hsb.B;
            double hue2 = hsb.H / 60;
            double x = chroma * (1 - Math.Abs(hue2 % 2 - 1));
            double r1 = 0d;
            double g1 = 0d;
            double b1 = 0d;
            if (hue2 >= 0 && hue2 < 1)
            {
                r1 = chroma;
                g1 = x;
            }
            else if (hue2 >= 1 && hue2 < 2)
            {
                r1 = x;
                g1 = chroma;
            }
            else if (hue2 >= 2 && hue2 < 3)
            {
                g1 = chroma;
                b1 = x;
            }
            else if (hue2 >= 3 && hue2 < 4)
            {
                g1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 4 && hue2 < 5)
            {
                r1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 5 && hue2 <= 6)
            {
                r1 = chroma;
                b1 = x;
            }
            double m = hsb.B - chroma;
            return new RGB()
            {
                R = r1 + m,
                G = g1 + m,
                B = b1 + m
            };
        }
        internal static HSB ConvertToHSB(RGB rgb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            double r = rgb.R;
            double g = rgb.G;
            double b = rgb.B;

            double max = Max(r, g, b);
            double min = Min(r, g, b);
            double chroma = max - min;
            double hue2 = 0d;
            if (chroma != 0)
            {
                if (max == r)
                {
                    hue2 = (g - b) / chroma;
                }
                else if (max == g)
                {
                    hue2 = (b - r) / chroma + 2;
                }
                else
                {
                    hue2 = (r - g) / chroma + 4;
                }
            }
            double hue = hue2 * 60;
            if (hue < 0)
            {
                hue += 360;
            }
            double brightness = max;
            double saturation = 0;
            if (chroma != 0)
            {
                saturation = chroma / brightness;
            }
            return new HSB()
            {
                H = hue,
                S = saturation,
                B = brightness
            };
        }
        private static double Max(double d1, double d2, double d3)
        {
            if (d1 > d2)
            {
                return Math.Max(d1, d3);
            }
            return Math.Max(d2, d3);
        }
        private static double Min(double d1, double d2, double d3)
        {
            if (d1 < d2)
            {
                return Math.Min(d1, d3);
            }
            return Math.Min(d2, d3);
        }

        internal struct RGB
        {
            internal double R;
            internal double G;
            internal double B;
        }

        internal struct HSB
        {
            internal double H;
            internal double S;
            internal double B;
        }
    }
}
