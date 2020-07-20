using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FanFiction.Data
{
	public class GoogleDriveRepository
	{
		private string[] Scopes = { DriveService.Scope.Drive };
		private string ApplicationName = "FanFiction";
		private string mainFolderId = "1m6IFk63GMFKo1S4effLP4MDqy-DPKAob";
		private string urlTemplate = "https://drive.google.com/uc?export=download&id=";

		//private string CreateFolder(string folderName)
		//{
		//	DriveService service = GetDriveServiceInstance();

		//	Google.Apis.Drive.v3.Data.File FileMetaData = new Google.Apis.Drive.v3.Data.File()
		//	{
		//		Name = folderName,
		//		MimeType = "application/vnd.google-apps.folder"
		//	};

		//	FilesResource.CreateRequest request;

		//	request = service.Files.Create(FileMetaData);
		//	request.Fields = "id";
		//	var file = request.Execute();
		//	return file.Id;
		//}

		public void DownloadFile(string blobId, string savePath)
		{
			var service = GetDriveServiceInstance();
			var request = service.Files.Get(blobId);
			var stream = new MemoryStream();
			// Add a handler which will be notified on progress changes.
			// It will notify on each chunk download and when the
			// download is completed or failed.
			request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
			{
				switch (progress.Status)
				{
					case Google.Apis.Download.DownloadStatus.Downloading:
						{
							Console.WriteLine(progress.BytesDownloaded);
							break;
						}
					case Google.Apis.Download.DownloadStatus.Completed:
						{
							Console.WriteLine("Download complete.");
							SaveStream(stream, savePath);
							break;
						}
					case Google.Apis.Download.DownloadStatus.Failed:
						{
							Console.WriteLine("Download failed.");
							break;
						}
				}
			};
			request.Download(stream);
		}

		public string GetImageLink(string imageId)
		{
			return urlTemplate + imageId;
		}

		public string UploadFIle(string fileName, Stream stream)
		{
			var service = GetDriveServiceInstance();
			var fileMetadata = new Google.Apis.Drive.v3.Data.File();
			fileMetadata.Name = fileName;
			fileMetadata.MimeType = "image/jpeg";
			fileMetadata.Parents = new List<string>()
			{
				mainFolderId
			};

			FilesResource.CreateMediaUpload request;

			request = service.Files.Create(fileMetadata, stream, "image/jpeg");
			request.Fields = "id";
			request.Upload();

			var file = request.ResponseBody;

			return file.Id;
		}

		private DriveService GetDriveServiceInstance()
		{
			UserCredential credential;

			using (var stream =
				new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

				credPath = Path.Combine(credPath, "./credentials/credentials.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
			}

			var service = new DriveService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			return service;
		}

		private static void SaveStream(MemoryStream stream, string saveTo)
		{
			using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
			{
				stream.WriteTo(file);
			}
		}

		public void DeleteFile(string filePath)
		{
			var service = GetDriveServiceInstance();

			var request = service.Files.Delete(filePath.Substring(urlTemplate.Length));

			request.Execute();
		}
	}
}
