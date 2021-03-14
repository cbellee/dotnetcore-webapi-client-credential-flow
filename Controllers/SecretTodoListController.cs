// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;
using TodoList_WebApi.Models;

namespace TodoList_WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecretTodoListController : ControllerBase
    {
        // In-memory TodoList
        private static readonly Dictionary<int, TodoItem> TodoStore = new Dictionary<int, TodoItem>();

        public SecretTodoListController()
        {
            // Pre-populate with sample data
            if (TodoStore.Count == 0)
            {
                TodoStore.Add(1, new TodoItem() { Id = 1, Task = "Buy secret sauce" });
                TodoStore.Add(2, new TodoItem() { Id = 2, Task = "Finish secret invoice report" });
                TodoStore.Add(3, new TodoItem() { Id = 3, Task = "Secret Water plants" });
            }
        }

        // GET: api/secrettodolist
        [HttpGet]
        public IActionResult Get()
        {
            // List the caller's claims
            List<Claim> claims = HttpContext.User.Claims.ToList();
            foreach (Claim claim in claims) {
                Console.WriteLine($"claim: {claim}");
            }
            
            // validate that the caller has the required claim (App Rols) assigned
            try
            {
                HttpContext.ValidateAppRole("SecretDaemonAppRole");
                return Ok(TodoStore.Values);
            } catch (Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
                return Forbid();
            }
        }
    }
}