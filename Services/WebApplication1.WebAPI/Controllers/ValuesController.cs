using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.WebAPI.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ValuesController : ControllerBase
    {
        private static List<string> __list = Enumerable
            .Range(1, 10)
            .Select(i => $"Значение-{i}")
            .ToList();

        //public IEnumerable<string> Get() => __list;
        //[HttpGet] public ActionResult<IList<string>> Get() => __list;

        [HttpGet]
        public IActionResult Get() => Ok(__list);

        [HttpGet("count")]
        public IActionResult GetCount() => Ok(__list.Count);

        [HttpGet("{id:int}")]
        [HttpGet("id[[{id:int}]]")]
        public IActionResult GetById(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id > __list.Count)
                return NotFound();
            return Ok(__list[id]);
        }

        [HttpGet("add")]
        public IActionResult Add(string str)
        {
            __list.Add(str);
            return Ok();
        }

        [HttpPut("{id:int}")]
        [HttpPut("edit/{id:int}")]
        public IActionResult Replace(int id, string str)
        {
            if (id < 0)
                return BadRequest();
            if (id > __list.Count)
                return NotFound();
            __list[id] = str;
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();
            if (id > __list.Count)
                return NotFound();
            __list.RemoveAt(id);
            return Ok();
        }

        [HttpGet("throw")]
        public IActionResult Throw(string message)
        {
            throw new ApplicationException(message ?? "Test Error in Values Controller");
        }
    }
}
