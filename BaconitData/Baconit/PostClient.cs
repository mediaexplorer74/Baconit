// Type: Baconit.PostClient
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Baconit
{
  public class PostClient
  {
    private StringBuilder _postData = new StringBuilder();
    public CookieContainer CookieContainer;
    public bool addSpecialHeaders;

    public event PostClient.DownloadStringCompletedHandler DownloadStringCompleted;

    public PostClient(string parameters) => this._postData.Append(parameters);

    public PostClient(IList<string> parameters)
    {
      foreach (object parameter in (IEnumerable<string>) parameters)
        this._postData.Append(string.Format("{0}&", parameter));
    }

    public PostClient(IDictionary<string, object> parameters)
    {
      foreach (KeyValuePair<string, object> parameter in (IEnumerable<KeyValuePair<string, object>>) parameters)
        this._postData.Append(string.Format("{0}={1}&", (object) parameter.Key, parameter.Value));
    }

    public void DownloadStringAsync(Uri address)
    {
      try
      {
        HttpWebRequest state = (HttpWebRequest) WebRequest.Create(address);
        state.Method = "POST";
        state.ContentType = "application/x-www-form-urlencoded";
        if (this.CookieContainer != null)
          state.CookieContainer = this.CookieContainer;
        state.BeginGetRequestStream(new AsyncCallback(this.RequestReady), (object) state);
      }
      catch
      {
        if (this.DownloadStringCompleted == null)
          return;
        this.DownloadStringCompleted((object) this, new DownloadStringCompletedEventArgs(new Exception("Error creating HTTP web request.")));
      }
    }

    private void RequestReady(IAsyncResult asyncResult)
    {
      HttpWebRequest asyncState = asyncResult.AsyncState as HttpWebRequest;
      using (Stream requestStream = asyncState.EndGetRequestStream(asyncResult))
      {
        using (StreamWriter streamWriter = new StreamWriter(requestStream))
        {
          streamWriter.Write(this._postData.ToString());
          streamWriter.Flush();
        }
      }
      asyncState.BeginGetResponse(new AsyncCallback(this.ResponseReady), (object) asyncState);
    }

    private void ResponseReady(IAsyncResult asyncResult)
    {
      HttpWebRequest asyncState = asyncResult.AsyncState as HttpWebRequest;
      try
      {
        HttpWebResponse response = (HttpWebResponse) asyncState.EndGetResponse(asyncResult);
        string result = string.Empty;
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            result = streamReader.ReadToEnd();
        }
        if (this.DownloadStringCompleted == null)
          return;
        this.DownloadStringCompleted((object) this, new DownloadStringCompletedEventArgs(result));
      }
      catch (Exception ex)
      {
        if (this.DownloadStringCompleted == null)
          return;
        this.DownloadStringCompleted((object) this, new DownloadStringCompletedEventArgs(ex));
      }
    }

    public delegate void DownloadStringCompletedHandler(
      object sender,
      DownloadStringCompletedEventArgs e);
  }
}
