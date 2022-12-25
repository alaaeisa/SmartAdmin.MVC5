using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Controllers
{
    public class BaseController : Controller
    {
        // GET: BaseController
        public string UploadFile()
        {

            HttpFileCollectionBase files = HttpContext.Request.Files;
            string filePath;
            string filePathTemp;
            string _FileName = "";
            for (int i = 0; i < files.Count; i++)
            {
                _FileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[0].FileName);
                filePath = Server.MapPath("~/Attachment/" + _FileName);
                files[i].SaveAs(filePath);
                //files[i].SaveAs(filePathTemp);

            }
            return _FileName;
        }

        public string UploadMultiFile()
        {

            HttpFileCollectionBase files = HttpContext.Request.Files;
            string filePath;
            string fileOne;
            string _FileName = "";
            for (int i = 0; i < files.Count; i++)
            {
                fileOne = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(files[i].FileName);
                _FileName += fileOne + "_";
                filePath = Server.MapPath("~/Attachment/" + fileOne);
                // filePathTemp = Server.MapPath("~/TempUpload/" + _FileName);
                files[i].SaveAs(filePath);
                //files[i].SaveAs(filePathTemp);

            }
            string V = _FileName.Remove(_FileName.Length - 1, 1);
            return V;
        }

        // string Is = ConfigurationManager.AppSettings["ttt"].ToString();
        public void SaveToMainFolder0()
        {
            string ImgFullPath = ConfigurationManager.AppSettings["ImgFullPath"].ToString();
            string IsFullPath = ConfigurationManager.AppSettings["IsFullPath"].ToString();

            string fileName = "";
            string destFile = "";
            string sourcePath = Server.MapPath("~/TempUpload/");
            string targetPath = "";
            if (IsFullPath == "1")
            {
                targetPath = ImgFullPath;
            }
            else
            {
                targetPath = Server.MapPath("~/" + ImgFullPath + "");
            }

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                int i = 0;
                foreach (string file in files)
                {
                    fileName = System.IO.Path.GetFileName(file);
                    destFile = System.IO.Path.Combine(targetPath, fileName);

                    System.IO.File.Copy(file, destFile, true);
                    i++;
                }

                RemoveFiles();

            }

        }

        public void RemoveFiles()
        {
            string sourcePath = Server.MapPath("~/TempUpload/");
            string[] files = System.IO.Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(sourcePath, file)))
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (System.IO.IOException e)
                    {
                        return;
                    }
                }
            }
        }
    }
}