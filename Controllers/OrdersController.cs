using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MigrationTestApp.Data;
using MigrationTestApp.Models;
using Azure.Storage.Blobs;

namespace MigrationTestApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private const string ContainerName = "invoices";

        public OrdersController(AppDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(100)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        public async Task<IActionResult> Create(
            long customerId,
            long productId,
            int quantity,
            IFormFile? invoiceFile)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return BadRequest("Invalid product.");

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = product.Price * quantity
            };

            order.OrderItems.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            });

            if (invoiceFile != null && invoiceFile.Length > 0)
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobName = $"{Guid.NewGuid()}-{invoiceFile.FileName}";
                var blobClient = containerClient.GetBlobClient(blobName);

                using var stream = invoiceFile.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);

                order.StorageBlobRef = blobName;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _context.TransactionLogs.Add(new TransactionLog
            {
                OrderId = order.Id,
                EventType = "OrderCreated",
                EventPayload = $"{{\"customerId\":{customerId},\"productId\":{productId},\"quantity\":{quantity}}}",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            await _context.SaveChangesAsync();

            _context.TransactionLogs.Add(new TransactionLog
            {
                OrderId = order.Id,
                EventType = "OrderStatusUpdated",
                EventPayload = $"{{\"newStatus\":\"{status}\"}}",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}