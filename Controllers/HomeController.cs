using ManagementApplication.Data;
using ManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ManagementApplication.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;



            // Инициализация БД
            if (db.Providers.Count() == 0)
            {
                // Таблица Поставщиков
                List<Provider> providers = new List<Provider>
                {
                    new Provider { Name = "ТПК ФЛЕКС" },
                    new Provider { Name = "U-Technology Group" },
                    new Provider { Name = "FJB GROUP LLC" },
                    new Provider { Name = "LLC VG-TRADE" },
                    new Provider { Name = "The KOOP Group" }
                };
                db.AddRange(providers);
                db.SaveChanges();


                // Таблица Заказов
                List<Order> orders = new List<Order>()
                {
                    new Order() { Number = "D1-420", Date = DateTime.Parse(DateTime.Now.AddDays(-20).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 1, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Швеллер", Quantity = 292.4M, Unit = "Кг" },
                        new OrderItem { Name = "Двутавр", Quantity = 349.0M, Unit = "Кг" },
                        new OrderItem { Name = "Балка", Quantity = 2M, Unit = "Шт" },
                    } },
                    new Order() { Number = "S6-521", Date = DateTime.Parse(DateTime.Now.AddDays(-14).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 4, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Анкерная плита", Quantity = 2M, Unit = "Шт" },
                        new OrderItem { Name = "Ригель", Quantity = 5M, Unit = "Шт" },
                    } },
                    new Order() { Number = "E4-457", Date = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 4, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Лист ПВЛ", Quantity = 30.8M, Unit = "Кг" },
                    } },
                    new Order() { Number = "G8-787", Date = DateTime.Parse(DateTime.Now.AddDays(-8).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 4, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Опора подвижная", Quantity = 30M, Unit = "Шт" },
                        new OrderItem { Name = "Опора трубопроводов", Quantity = 18M, Unit = "Шт" },
                    } },
                    new Order() { Number = "B4-128", Date = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 2, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Песок", Quantity = 90.4M, Unit = "Кг" },
                        new OrderItem { Name = "Щебень", Quantity = 150.0M, Unit = "Кг" },
                        new OrderItem { Name = "Керамзит", Quantity = 64.0M, Unit = "Кг" },
                    } },
                    new Order() { Number = "A2-574", Date = DateTime.Parse(DateTime.Now.AddDays(-6).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 1, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Двутавр", Quantity = 56.0M, Unit = "Кг" },
                        new OrderItem { Name = "Швеллер", Quantity = 140.4M, Unit = "Кг" },
                    } },
                    new Order() { Number = "C6-134", Date = DateTime.Parse(DateTime.Now.AddDays(-4).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 3, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Ригель", Quantity = 3M, Unit = "Шт" },
                    } },
                    new Order() { Number = "T1-420", Date = DateTime.Parse(DateTime.Now.AddDays(-3).ToString("yyyy-MM-ddTHH:mm")), ProviderId = 5, OrderItems = new List<OrderItem>() {
                        new OrderItem { Name = "Плита перекрытия", Quantity = 7M, Unit = "Шт" },
                    } }
                };
                db.AddRange(orders);
                db.SaveChanges();
            }

        }




        public async Task<IActionResult> Index(FilterView filterView)
        {

            // Период времени по умолчанию
            if (filterView.SelectedStartDate == null) filterView.SelectedStartDate = DateTime.Now.AddMonths(-1);
            if (filterView.SelectedEndDate == null) filterView.SelectedEndDate = DateTime.Now;

            // Список Номеров Заказов
            filterView.Orders = await db.Orders.AsNoTracking().Select(p => new SelectListItem() { Text = p.Number, Value = p.Number }).Distinct().ToListAsync();
            if (filterView.SelectedNumbers == null) filterView.SelectedNumbers = new string[] { "" };

            // Список Наименований Поставщиков
            filterView.Providers = await db.Providers.AsNoTracking().Select(p => new SelectListItem() { Text = p.Name, Value = p.Name }).Distinct().ToListAsync();
            if (filterView.SelectedProviders == null) filterView.SelectedProviders = new string[] { "" };

            // Список Названий Элементов заказа
            filterView.ItemNames = await db.OrderItems.AsNoTracking().Select(p => new SelectListItem() { Text = p.Name, Value = p.Name }).Distinct().ToListAsync();
            if (filterView.SelectedName == null) filterView.SelectedName = new string[] { "" };

            // Список Названий Единиц измерений Элементов заказа
            filterView.ItemUnits = await db.OrderItems.AsNoTracking().Select(p => new SelectListItem() { Text = p.Unit, Value = p.Unit }).Distinct().ToListAsync();
            if (filterView.SelectedUnit == null) filterView.SelectedUnit = new string[] { "" };


            var table = await (from order in db.Orders
                               join provider in db.Providers on order.ProviderId equals provider.Id
                               join orderItem in db.OrderItems on order.Id equals orderItem.OrderId into tempOrderItems
                               from tempOrderItem in tempOrderItems.DefaultIfEmpty()
                               group new { order, provider, tempOrderItem } by order.Id into g
                               select new OrderProviderItem
                               {
                                   Id = g.Key,
                                   Number = g.Select(p => p.order.Number).FirstOrDefault(),
                                   Date = g.Select(p => p.order.Date).FirstOrDefault(),
                                   ProviderName = g.Select(p => p.provider.Name).FirstOrDefault(),
                                   OrderItems = g.Select(p => p.tempOrderItem).ToList()
                               }).AsNoTracking().OrderByDescending(t => t.Id).ToListAsync();

            filterView.OrderProviderItems = table;


            //Фильтр по Датам
            if (filterView.SelectedStartDate != null && filterView.SelectedEndDate != null)
            {
                filterView.OrderProviderItems = filterView.OrderProviderItems.Where(p => p.Date >= filterView.SelectedStartDate).Where(p => p.Date <= filterView.SelectedEndDate).ToList();
            }

            // Фильтр по Номеру заказа
            if (filterView.SelectedNumbers.Length != 1 || String.IsNullOrEmpty(filterView.SelectedNumbers[0]) != true)
            {
                filterView.OrderProviderItems = filterView.OrderProviderItems.Where(p => filterView.SelectedNumbers.Contains(p.Number, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            // Фильтр по Поставщику
            if (filterView.SelectedProviders.Length != 1 || String.IsNullOrEmpty(filterView.SelectedProviders[0]) != true)
            {
                filterView.OrderProviderItems = filterView.OrderProviderItems.Where(p => filterView.SelectedProviders.Contains(p.ProviderName, StringComparer.OrdinalIgnoreCase)).ToList();
            }

            // Фильтр по Названию элемента заказа
            if (filterView.SelectedName.Length != 1 || String.IsNullOrEmpty(filterView.SelectedName[0]) != true)
            {
                filterView.OrderProviderItems = filterView.OrderProviderItems.Where(p => p.OrderItems.Where(i => filterView.SelectedName.Contains(i.Name, StringComparer.OrdinalIgnoreCase)).Count() > 0).ToList();
            }

            // Фильтр по Названию Единицы измерения Элементов заказа
            if (filterView.SelectedUnit.Length != 1 || String.IsNullOrEmpty(filterView.SelectedUnit[0]) != true)
            {
                filterView.OrderProviderItems = filterView.OrderProviderItems.Where(p => p.OrderItems.Where(i => filterView.SelectedUnit.Contains(i.Unit, StringComparer.OrdinalIgnoreCase)).Count() > 0).ToList();
            }

            return View(filterView);
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Orders == null || db.Providers == null || db.OrderItems == null)
            {
                return NotFound();
            }

            var table = await (from order in db.Orders
                               where order.Id == id
                               join provider in db.Providers on order.ProviderId equals provider.Id
                               join orderItem in db.OrderItems on order.Id equals orderItem.OrderId into tempOrderItems
                               from tempOrderItem in tempOrderItems.DefaultIfEmpty()
                               group new { order, provider, tempOrderItem } by order.Id into g
                               select new OrderProviderItem
                               {
                                   Id = g.Key,
                                   Number = g.Select(p => p.order.Number).FirstOrDefault(),
                                   Date = g.Select(p => p.order.Date).FirstOrDefault(),
                                   ProviderName = g.Select(p => p.provider.Name).FirstOrDefault(),
                                   OrderItems = g.Select(p => p.tempOrderItem).ToList()
                               }).AsNoTracking().FirstOrDefaultAsync();

            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }




        public async Task<IActionResult> Create()
        {
            ViewBag.ProvidersList = await db.Providers.Distinct().ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            ViewBag.ProvidersList = await db.Providers.Distinct().ToListAsync();

            // Определение Номера заказа и Названия элемента на идентичность
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Name == order.Number)
                    {
                        ModelState.AddModelError("Number", "'Номер заказа' и 'Название элемента заказа' не должны быть идентичны!");
                    }
                }
            }

            // Поиск заказа в БД с таким же номером и поставщиком.
            var duplicate = await db.Orders.Where(p => p.Number == order.Number).FirstOrDefaultAsync(p => p.ProviderId == order.ProviderId);
            if (duplicate != null)
            {
                ModelState.AddModelError("order.Number", "Измените 'Номер заказа'! Заказ с таким номером и поставщиком уже существует!");
            }

            if (ModelState.IsValid == true)
            {
                db.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order);
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Orders == null)
            {
                return NotFound();
            }

            var order = await db.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (db.Orders == null)
            {
                return NotFound();
            }

            var order = await db.Orders.FindAsync(id);
            if (order != null)
            {
                db.Orders.Remove(order);
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.ProvidersList = await db.Providers.Distinct().ToListAsync();

            if (id == null || db.Orders == null || db.OrderItems == null || db.Providers == null)
            {
                return NotFound();
            }

            Order? order = await (from o in db.Orders
                                  where o.Id == id
                                  join oi in db.OrderItems on o.Id equals oi.OrderId into temps
                                  from temp in temps.DefaultIfEmpty()
                                  group new { o, temp } by o.Id into g
                                  select new Order
                                  {
                                      Id = g.Key,
                                      Number = g.Select(p => p.o.Number).FirstOrDefault(),
                                      Date = g.Select(p => p.o.Date).FirstOrDefault(),
                                      ProviderId = g.Select(p => p.o.ProviderId).FirstOrDefault(),
                                      OrderItems = g.Select(p => p.temp).ToList()
                                  }).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            ViewBag.ProvidersList = await db.Providers.Distinct().ToListAsync();

            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(order);
                    await db.SaveChangesAsync();

                    // Удаление отсутствующих в модели элементов
                    var listItems = await db.OrderItems.AsNoTracking().Where(p => p.OrderId == id).ToListAsync();
                    var lost = listItems.Where(i => order.OrderItems.Select(p => p.Id).Contains(i.Id) != true).ToList();

                    if (lost != null)
                    {
                        db.OrderItems.RemoveRange(lost);
                        await db.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (OrderIsExists(order.Id) != true)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(order);
        }


        private bool OrderIsExists(int id)
        {
            return (db.Orders?.Any(p => p.Id == id)).GetValueOrDefault();
        }



    }
}