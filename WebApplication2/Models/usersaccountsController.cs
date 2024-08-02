using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class usersaccountsController : Controller
    {
        private readonly WebApplication2Context _context;

        public usersaccountsController(WebApplication2Context context)
        {
            _context = context;
        }
        

        // GET: usersaccounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.usersaccounts.ToListAsync());
        }

        // GET: usersaccounts/Details/5
        
        // GET: usersaccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ActionName("login")]

        [ValidateAntiForgeryToken]

        public IActionResult Login(string na,string pa, Microsoft.Data.SqlClient.SqlConnection SqlConnection)

        {


            Microsoft.Data.SqlClient.SqlConnection conn1 =new("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\aiman\\OneDrive\\Documents\\mynewdb3.mdf;Integrated Security=True;Connect Timeout=30");

            string sql;

            sql = "SELECT * FROM usersaccounts where name ='" + na + "' and  pass ='" + pa + "' ";

            Microsoft.Data.SqlClient.SqlCommand comm = new(sql, conn1);

            conn1.Open();

            Microsoft.Data.SqlClient.SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())

            {

                string role = (string)reader["role"];
                string id = Convert.ToString((int)reader["Id"]);

                HttpContext.Session.SetString("Name", na);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("userid", id);
                reader.Close();

                conn1.Close();

                if (role == "customer")

                    return RedirectToAction("catalogue", "books");

                else

                    return RedirectToAction("Index", "books");

            }

            else

            {

                ViewData["Message"] = "wrong user name password";

                return View();

            }

        }






        // POST: usersaccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,pass,email")] usersaccounts usersaccounts)
        {
            usersaccounts.role = "customer";
                _context.Add(usersaccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login ));
           
        }

        // GET: usersaccounts/Edit/5
        public async Task<IActionResult> Edit()
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            var usersaccounts = await _context.usersaccounts.FindAsync(id);
            
            return View(usersaccounts);
        }

        // POST: usersaccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,name,pass,role,email")]usersaccounts usersaccounts)
        {
            
                    _context.Update(usersaccounts);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Login ));
           
        }

        // GET: usersaccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersaccounts = await _context.usersaccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersaccounts == null)
            {
                return NotFound();
            }

            return View(usersaccounts);
        }

       

        private bool UsersaccountsExists(int id)
        {
            return _context.usersaccounts.Any(e => e.Id == id);
        }
    }
}
