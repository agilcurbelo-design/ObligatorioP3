using Obligatorio.AccesoDatos.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Obligatorio.LogicaNegocio.Entidades;
using Obligatorio.AccesoDatos;

namespace Obligatorio.WebApp2.Controllers
{


	public class TipoGastoController : Controller
	{
		private readonly OblContext _context;

		public TipoGastoController(OblContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View(_context.TipoGastos.ToList());
		}

		public IActionResult Create() => View();

		[HttpPost]
		public IActionResult Create(TipoGasto tipoGasto)
		{
			if (ModelState.IsValid)
			{
				_context.TipoGastos.Add(tipoGasto);
				_context.SaveChanges();
				RegistrarAuditoria("Alta", tipoGasto.Id);
				return RedirectToAction("Index");
			}
			return View(tipoGasto);
		}

		public IActionResult Edit(int id)
		{
			var tipoGasto = _context.TipoGastos.Find(id);
			return View(tipoGasto);
		}

		[HttpPost]
		public IActionResult Edit(TipoGasto tipoGasto)
		{
			if (ModelState.IsValid)
			{
				_context.TipoGastos.Update(tipoGasto);
				_context.SaveChanges();
				RegistrarAuditoria("Edición", tipoGasto.Id);
				return RedirectToAction("Index");
			}
			return View(tipoGasto);
		}

		public IActionResult Delete(int id)
		{
			var tipoGasto = _context.TipoGastos.Find(id);

			bool enUso = _context.Pagos.Any(p => p.TipoGastoId == id);
			if (enUso)
			{
				TempData["Error"] = "No se puede eliminar: el tipo de gasto está en uso.";
				return RedirectToAction("Index");
			}

			_context.TipoGastos.Remove(tipoGasto);
			_context.SaveChanges();
			RegistrarAuditoria("Baja", tipoGasto.Id);
			return RedirectToAction("Index");
		}

		private void RegistrarAuditoria(string accion, int tipoGastoId)
		{
			var usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId"));
			_context.Auditorias.Add(new Auditoria
			{
				Accion = accion,
				Fecha = DateTime.Now,
				UsuarioId = usuarioId
			});
			_context.SaveChanges();
		}
	}

}



