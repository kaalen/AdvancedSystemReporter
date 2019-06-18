using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Shell.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.IO;
using Sitecore.Text;
using Sitecore.Data;
using System.IO;
using Sitecore.Configuration;
using Sitecore.Globalization;
using Sitecore.Web;
using Sitecore.Data.Items;

namespace Sitecore.Support.Shell
{
    public class DownloadPage : SecurePage
    {
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            string text = StringUtil.GetString(new string[]
            {
                base.Request.QueryString["file"]
            });
            bool flag = false;
            string filename = FileHandle.GetFilename(text);
            if (!string.IsNullOrEmpty(filename))
            {
                text = filename;
                flag = true;
            }
            if (!string.IsNullOrEmpty(text))
            {

                #region this code has been added to fix 90534
                PageScriptManager pageScriptManager = this.Context.Items["sc_pagescriptmanager"] as PageScriptManager;
                if (pageScriptManager != null)
                {
                    pageScriptManager.AddStylesheets = false;
                }
                #endregion
                if (MediaManager.IsMediaUrl(text))
                {
                    this.DownloadMediaFile(text);
                    return;
                }
                if (text.IndexOf("id=", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.DownloadMediaById(text);
                    return;
                }
                if (flag || Sitecore.Context.IsAdministrator)
                {
                    this.DownloadFile(text);
                }
            }
        }

        /// <summary>
        /// Executes the file event.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <contract>
        ///   <requires name="file" condition="not null" />
        /// </contract>
        private void DownloadFile(string file)
        {
            Assert.ArgumentNotNull(file, "file");
            file = FileUtil.MapPath(file);
            string value = FileUtil.MapPath("/");
            string value2 = FileUtil.MapPath(Settings.DataFolder);
            if (!file.StartsWith(value, StringComparison.InvariantCultureIgnoreCase) && !file.StartsWith(value2, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            if (!File.Exists(file))
            {
                return;
            }
            FileInfo fileInfo = new FileInfo(file);
            this.WriteCacheHeaders(Path.GetFileName(file), fileInfo.Length);
            base.Response.TransmitFile(file);
        }

        /// <summary>
        /// Executes the media by id event.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <contract>
        ///   <requires name="file" condition="not null" />
        /// </contract>
        private void DownloadMediaById(string file)
        {
            Assert.ArgumentNotNull(file, "file");
            UrlString urlString = new UrlString(System.Web.HttpUtility.UrlDecode(file));
            ItemUri uri = new ItemUri(urlString["id"], Language.Parse(urlString["la"]), Sitecore.Data.Version.Parse(urlString["vs"]), StringUtil.GetString(new string[]
            {
                urlString["db"],
                Sitecore.Context.ContentDatabase.Name
            }));
            Item item = Database.GetItem(uri);
            if (item == null)
            {
                return;
            }
            this.WriteMediaItem(item);
        }

        /// <summary>
        /// Executes the media file event.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <contract>
        ///   <requires name="file" condition="not null" />
        /// </contract>
        private void DownloadMediaFile(string file)
        {
            Assert.ArgumentNotNull(file, "file");
            string text = null;
            int num = -1;
            foreach (string current in MediaManager.Config.MediaPrefixes)
            {
                num = file.IndexOf(current, StringComparison.OrdinalIgnoreCase);
                if (num >= 0)
                {
                    text = current;
                    break;
                }
            }
            Assert.IsNotNull(text, "media prefix not found.");
            file = StringUtil.Mid(file, num + text.Length);
            num = file.LastIndexOf('?');
            if (num >= 0)
            {
                file = StringUtil.Left(file, num);
            }
            num = file.LastIndexOf('.');
            if (num >= 0)
            {
                file = StringUtil.Left(file, num);
            }
            Item item = null;
            if (ShortID.IsShortID(file))
            {
                item = Sitecore.Context.ContentDatabase.GetItem(new ShortID(file).ToID());
            }
            if (item == null && Sitecore.Data.ID.IsID(file))
            {
                item = Sitecore.Context.ContentDatabase.GetItem(file);
            }
            if (item == null)
            {
                if (!file.StartsWith("/", StringComparison.InvariantCulture))
                {
                    file = "/" + file;
                }
                file = FileUtil.MakePath("/sitecore/media library", file);
                item = Sitecore.Context.ContentDatabase.GetItem(file);
            }
            if (item == null)
            {
                return;
            }
            this.WriteMediaItem(item);
        }

        /// <summary>
        /// Writes the cache headers.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="length">The length.</param>
        /// <contract>
        ///   <requires name="filename" condition="not null" />
        /// </contract>
        private void WriteCacheHeaders(string filename, long length)
        {
            Assert.ArgumentNotNull(filename, "filename");
            base.Response.ClearHeaders();
            base.Response.AddHeader("Content-Type", "application/octet-stream");
            base.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            base.Response.AddHeader("Content-Length", length.ToString());
            base.Response.AddHeader("Content-Transfer-Encoding", "binary");
            base.Response.CacheControl = "private";
        }

        /// <summary>
        /// Writes the media item.
        /// </summary>
        /// <param name="mediaItem">The media item.</param>
        /// <contract>
        ///   <requires name="mediaItem" condition="not null" />
        /// </contract>
        protected void WriteMediaItem(MediaItem mediaItem)
        {
            Assert.ArgumentNotNull(mediaItem, "mediaItem");
            Stream mediaStream = mediaItem.GetMediaStream();
            string text = mediaItem.Extension;
            if (!text.StartsWith(".", StringComparison.InvariantCulture))
            {
                text = "." + text;
            }
            this.WriteCacheHeaders(mediaItem.Name + text, mediaStream.Length);
            WebUtil.TransmitStream(mediaStream, base.Response, Settings.Media.StreamBufferSize);
        }
    }
}