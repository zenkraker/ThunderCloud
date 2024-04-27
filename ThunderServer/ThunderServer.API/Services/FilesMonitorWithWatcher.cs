using k8s;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using ThunderServer.API.Configurations;

namespace ThunderServer.API.Services
{
	public class FilesMonitorWithWatcher : BackgroundService
	{
		//private readonly ThunderServerContext _serverContext;
		private readonly StorageVolumesConfiguration _configuration;
		private readonly ILogger<FilesMonitorWithWatcher> _logger;
		private static readonly string _fileFilter = Path.Combine("", "*.*");
		private static PhysicalFileProvider _fileProvider;
		private static IChangeToken _fileChangeToken;

		//It seems to be broken when monitoring a folder mounted as docker volume
		public FilesMonitorWithWatcher(IOptionsMonitor<StorageVolumesConfiguration> optionsMonitor, ILogger<FilesMonitorWithWatcher> logger)
		{
			//this._serverContext = serverContext;
			this._configuration = optionsMonitor.CurrentValue;
			this._logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			//var fileProvider = new PhysicalFileProvider(this._configuration.UploadFolder);
			//IChangeToken token = fileProvider.Watch(_fileFilter);

			_fileProvider = new PhysicalFileProvider(this._configuration.UploadFolder);



			while (!stoppingToken.IsCancellationRequested)
			{
				//		using var watcher = new FileSystemWatcher($@"{this._configuration.UploadFolder}");

				//		watcher.Filter = "*.*";
				//		watcher.IncludeSubdirectories = true;
				//		watcher.EnableRaisingEvents = true;

				//		watcher.NotifyFilter = NotifyFilters.Attributes
				//					 | NotifyFilters.CreationTime
				//					 | NotifyFilters.DirectoryName
				//					 | NotifyFilters.FileName
				//					 | NotifyFilters.LastAccess
				//					 | NotifyFilters.LastWrite
				//					 | NotifyFilters.Security
				//					 | NotifyFilters.Size;

				//		var test = new PhysicalFilesWatcher(this._configuration.UploadFolder, watcher, true);

				//		_fileChangeToken = test.CreateFileChangeToken("this._configuration.UploadFolder/**/*.*");

				//		//WatchForFileChanges();
				//	}

				//}
				using var watcher = new FileSystemWatcher($@"{this._configuration.UploadFolder}");


				watcher.Filter = "*.*";
				watcher.IncludeSubdirectories = true;
				watcher.EnableRaisingEvents = true;


				watcher.NotifyFilter = NotifyFilters.Attributes
									 | NotifyFilters.CreationTime
									 | NotifyFilters.DirectoryName
									 | NotifyFilters.FileName
									 | NotifyFilters.LastAccess
									 | NotifyFilters.LastWrite
									 //| NotifyFilters.Security
									 | NotifyFilters.Size;

				watcher.Changed += OnChanged;
				watcher.Created += OnCreated;
				watcher.Deleted += OnDeleted;
				watcher.Renamed += OnRenamed;
				watcher.Error += OnError;


				await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
			}
		}


		private static void WatchForFileChanges()
		{
			//_fileChangeToken = _fileProvider.Watch("*.*");

			

			_fileChangeToken.RegisterChangeCallback(Notify, default);
		}

		private static void Notify(object state)
		{
			

			Console.WriteLine("File change detected");
			WatchForFileChanges();
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			switch (e.ChangeType)
			{
				case WatcherChangeTypes.Deleted:
					OnDeleted(sender, e);
					break;
				case WatcherChangeTypes.Created:
					OnCreated(sender, e);
					break;
				default:
					ChangedMonitor(sender, e);
					break;
			}
		}
		private void ChangedMonitor(object sender, FileSystemEventArgs e)
		{
			FileAttributes attr = File.GetAttributes(e.FullPath);

			var isDirectory = false;

			//detect whether its a directory or file
			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
				isDirectory = true;

			if (isDirectory)
			{
				_logger.LogDebug("directory state changed to: {changeType} for: {fileName} at {filePath}", e.ChangeType, e.Name, e.FullPath);
			}
			else
			{
				_logger.LogDebug("File state changed to: {changeType} for: {fileName} at {filePath}", e.ChangeType, e.Name, e.FullPath);
			}
		}

		private void OnCreated(object sender, FileSystemEventArgs e)
		{
			_logger.LogDebug("Created file: {fileName} at {filePath}", e.Name, e.FullPath);
		}

		private void OnDeleted(object sender, FileSystemEventArgs e)
		{
			_logger.LogDebug("Deleted file: {fileName} at {filePath}", e.Name, e.FullPath);
		}

		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			_logger.LogDebug("Renamed file: from old: {oldFilePath} - new: {newFilePath}", e.OldFullPath, e.FullPath);
		}

		private void OnError(object sender, ErrorEventArgs e)
		{
			_logger.LogError("Error occured while performing operaiton on file: {message}", e.GetException());
		}
	}
}
