using System;
using System.IO;
using System.Text;
using System.Web;

public class HttpResponseLogWriter : TextWriter
{
    private readonly HttpResponseBase _response;

    public HttpResponseLogWriter(HttpResponseBase response)
    {
        this._response = response;
        _response.BufferOutput = false;

        response.Write("<html><body>");
        WriteLogEntry("Started at " + DateTime.Now.ToShortTimeString());

//            _response.BufferOutput = true;
    }

    public override void WriteLine(string value)
    {
        WriteLogEntry(value);

        try
        {
            _response.Flush();
        }
        catch
        {

        }
            
    }

    private void WriteLogEntry(string value)
    {
        try
        {
            _response.Write(value + " <br>");
        }
        catch 
        {
                
        }
    }

    public override Encoding Encoding
    {
        get { return Encoding.Default; }
    }
}