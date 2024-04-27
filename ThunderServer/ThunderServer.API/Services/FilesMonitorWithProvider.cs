using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ThunderServer.API.Configurations;

namespace ThunderServer.API.Services;

public class FilesMonitorWithProvider : BackgroundService
{
	private readonly StorageVolumesConfiguration _configuration;
	private readonly ILogger<FilesMonitorWithProvider> _logger;
	private readonly PhysicalFileProvider _physicalFileProvider;
	private readonly Dictionary<string, DateTime> _previousDirectoryState = new Dictionary<string, DateTime>();

	public FilesMonitorWithProvider(IOptionsMonitor<StorageVolumesConfiguration> optionsMonitor, ILogger<FilesMonitorWithProvider> logger)
	{
		//this._serverContext = serverContext;
		this._configuration = optionsMonitor.CurrentValue;
		this._logger = logger;

		_physicalFileProvider = new PhysicalFileProvider(this._configuration.UploadFolder);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			WatchForFileChanges();
		}

		return Task.CompletedTask;
	}

	private void Notify(object state)
	{
		_logger.LogDebug("File change detected");
		WatchForFileChanges();
	}

	private void WatchForFileChanges()
	{
		var _fileChangeToken = _physicalFileProvider.Watch("*.*");
		_fileChangeToken.RegisterChangeCallback(Notify, default);
	}

	static void OnDirectoryChange(object state)
	{
		var fileProvider = (PhysicalFileProvider)state;

		// Trigger the timer to start the periodic scan
		((Timer)state).Change(TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan); // Adjust the interval as needed
	}

	void CheckDirectoryChanges(object state)
	{
		var fileProvider = (PhysicalFileProvider)state;

		var currentDirectoryState = GetDirectoryState(fileProvider, string.Empty);

		if (DirectoryChanged(currentDirectoryState))
		{
			Console.WriteLine("Directory contents changed.");
			// Trigger appropriate callbacks based on detected changes
			HandleDirectoryChanges(currentDirectoryState);
		}

		_previousDirectoryState = currentDirectoryState;
	}

	Dictionary<string, DateTime> GetDirectoryState(PhysicalFileProvider fileProvider, string path)
	{
		var directoryState = new Dictionary<string, DateTime>();

		CollectFilesAndDirectories(fileProvider, path, directoryState);

		return directoryState;
	}

	void CollectFilesAndDirectories(PhysicalFileProvider fileProvider, string path, Dictionary<string, DateTime> directoryState)
	{
		var directoryContents = fileProvider.GetDirectoryContents(path);

		foreach (var fileInfo in directoryContents)
		{
			string fullPath = Path.Combine(path, fileInfo.Name);

			if (fileInfo.IsDirectory)
			{
				directoryState[fullPath] = fileInfo.LastModified.UtcDateTime;
				CollectFilesAndDirectories(fileProvider, fullPath, directoryState); // Recursively collect files and directories
			}
			else
			{
				directoryState[fullPath] = fileInfo.LastModified.UtcDateTime;
			}
		}
	}

	bool DirectoryChanged(Dictionary<string, DateTime> currentDirectoryState)
	{
		// Check if the current directory state is different from the previous state
		return !_previousDirectoryState.SequenceEqual(currentDirectoryState);
	}

	void HandleDirectoryChanges(Dictionary<string, DateTime> currentDirectoryState)
	{
		// Split the handling based on file or directory changes
		foreach (var kvp in currentDirectoryState)
		{
			var itemPath = kvp.Key;
			var lastModified = kvp.Value;

			if (!_previousDirectoryState.ContainsKey(itemPath))
			{
				// New file or directory
				if (Directory.Exists(itemPath))
				{
					HandleDirectoryCreated(itemPath, lastModified);
				}
				else
				{
					HandleFileCreated(itemPath, lastModified);
				}
			}
			else
			{
				var previousLastModified = _previousDirectoryState[itemPath];

				if (lastModified != previousLastModified)
				{
					// File or directory modified
					if (Directory.Exists(itemPath))
					{
						HandleDirectoryModified(itemPath, lastModified, previousLastModified);
					}
					else
					{
						HandleFileModified(itemPath, lastModified, previousLastModified);
					}
				}
			}
		}

		// Check for deleted files or directories
		foreach (var previousItemPath in _previousDirectoryState.Keys)
		{
			if (!currentDirectoryState.ContainsKey(previousItemPath))
			{
				if (Directory.Exists(previousItemPath))
				{
					HandleDirectoryDeleted(previousItemPath);
				}
				else
				{
					HandleFileDeleted(previousItemPath);
				}
			}
		}
	}

	void HandleDirectoryCreated(string path, DateTime lastModified)
	{
		this._logger.LogDebug("Directory created: ", path);
		// Implement actions to handle directory creation
	}

	void HandleDirectoryModified(string path, DateTime currentLastModified, DateTime previousLastModified)
	{
		_logger.LogDebug($"Directory modified: {path}");
		// Implement actions to handle directory modification
	}

	void HandleDirectoryDeleted(string path)
	{
		_logger.LogDebug($"Directory deleted: {path}");
		// Implement actions to handle directory deletion
	}

	void HandleFileCreated(string path, DateTime lastModified)
	{
		_logger.LogDebug($"File created: {path}");
		// Implement actions to handle file creation
	}

	void HandleFileModified(string path, DateTime currentLastModified, DateTime previousLastModified)
	{
		_logger.LogDebug($"File modified: {path}");
		// Implement actions to handle file modification
	}

	void HandleFileDeleted(string path)
	{
		_logger.LogDebug($"File deleted: {path}");
		// Implement actions to handle file deletion
	}
	