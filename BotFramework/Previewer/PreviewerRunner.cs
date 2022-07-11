using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using BotFramework.Previewer.HandlersMetadata;
using BotFramework.Previewer.HandlersMetadata.Interfaces;
using BotFramework.Previewer.Interfaces;

namespace BotFramework.Previewer;

/// <summary>
/// Стандартная реализация для <see cref="IPreviewerRunner"/>
/// </summary>
internal class PreviewerRunner : IPreviewerRunner
{
	/// <inheritdoc />
	public string MetadataSchema { get; }

	/// <summary>
	/// Инициализирует <see cref="IPreviewerRunner"/>
	/// </summary>
	/// <param name="metadataSerializer">Сериализатор метаданных для анализа</param>
	/// <param name="metadata">Метаданные для анализа</param>
	public PreviewerRunner(IMetadataSerializer metadataSerializer, AnalysisMetadata metadata)
	{
		ArgumentNullException.ThrowIfNull(metadataSerializer);
		ArgumentNullException.ThrowIfNull(metadata);

		MetadataSchema = metadataSerializer.Serialize(metadata);
	}

	/// <inheritdoc />
	public void Run()
	{
		try
		{
			var previewerProcess = StartPreviewer();
			DisposeProcessAfterExit(previewerProcess);
		}
		
		catch
		{
			throw new Exception
			(
				"Невозможно запустить BotFramework Previewer. " +
				"Пожалуйста, установите его с помощью следующей команды в терминале: 'dotnet tool install --global BotFramework.NET.Previewer'."
			);
		}
	}

	/// <summary>
	/// Запускает BotFramework Previewer
	/// </summary>
	private Process StartPreviewer()
	{
		var encodedSchema = Convert.ToBase64String
		(
			Encoding.UTF8.GetBytes(MetadataSchema)
		);
		
		return Process.Start
		(
			new ProcessStartInfo
			{
				FileName = "botframework-previewer",
				Arguments = encodedSchema,
				UseShellExecute = false,
				CreateNoWindow = true
			}
		);
	}

	/// <summary>
	/// Асинхронно выгружает ресурсы при завершении указанного процесса
	/// </summary>
	/// <param name="process">Процесс</param>
	private void DisposeProcessAfterExit(Process process)
	{
		Task.Run(async () =>
		{
			await process.WaitForExitAsync();
			process.Dispose();
		});
	}
}