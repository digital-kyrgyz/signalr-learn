using System.Data;
using System.Threading.Channels;
using Microsoft.Extensions.FileProviders;
using SignalR.Identity.Models;
using System.Data.Common;
using ClosedXML.Excel;
using Microsoft.AspNetCore.SignalR;
using SignalR.Identity.Hubs;

namespace SignalR.Identity.BackgroundService;

public class CreateExcelBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly Channel<(string, List<Product>)> _channel;
    private readonly IFileProvider _fileProvider;
    private readonly IServiceProvider _serviceProvider;

    public CreateExcelBackgroundService(Channel<(string, List<Product>)> channel, IFileProvider fileProvider,
        IServiceProvider serviceProvider)
    {
        _channel = channel;
        _fileProvider = fileProvider;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _channel.Reader.WaitToReadAsync(stoppingToken))
        {
            await Task.Delay(7000);
            var (userId, products) = await _channel.Reader.ReadAsync(stoppingToken);
            var wwwRootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var filesFolder = wwwRootFolder.Single(x => x.Name == "Files");
            var excelFileName = $"product-list-{Guid.NewGuid()}.xlsx";
            var excelFilePath = Path.Combine(filesFolder.PhysicalPath, excelFileName);
            //creating excel
            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(GetTable("Product List", products));
            wb.Worksheets.Add(ds);

            await using var excelFileStream = new FileStream(excelFilePath, FileMode.Create);
            wb.SaveAs(excelFileStream);

            using (var scope = _serviceProvider.CreateScope())
            {
                var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>();
                await appHub.Clients.User(userId).SendAsync("AlertCompleteFile", $"/Files/{excelFileName}", stoppingToken);
            }
        }
    }

    private DataTable GetTable(string tableName, List<Product> products)
    {
        var table = new DataTable { TableName = tableName };

        foreach (var item in typeof(Product).GetProperties()) table.Columns.Add(item.Name, item.PropertyType);
        products.ForEach(x => { table.Rows.Add(x.Id, x.Name, x.Price, x.Description, x.UserId); });
        return table;
    }
}