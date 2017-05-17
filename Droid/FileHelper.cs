using System;
using System.IO;
using TWallet.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace TWallet.Droid
{
	public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(path, filename);
		}
	}
}
