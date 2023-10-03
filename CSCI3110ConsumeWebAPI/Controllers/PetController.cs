using CSCI3110ConsumeWebAPI.Models;
using CSCI3110ConsumeWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSCI3110ConsumeWebAPI.Controllers;

public class PetController : Controller
{
    private readonly IPetRepository _petRepo;

    public PetController(IPetRepository petRepo)
    {
        _petRepo = petRepo;
    }

    public async Task<IActionResult> Index()
    {
        var pets = await _petRepo.ReadAllAsync();
        return View(pets);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pet = await _petRepo.ReadAsync(id);
        if (pet == null)
        {
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Pet pet)
    {
        if (ModelState.IsValid)
        {
            await _petRepo.CreateAsync(pet);
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pet = await _petRepo.ReadAsync(id);
        if (pet == null)
        {
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Pet pet)
    {
        if (ModelState.IsValid)
        {
            await _petRepo.UpdateAsync(pet.Id, pet);
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _petRepo.ReadAsync(id);
        if (pet == null)
        {
            return RedirectToAction("Index");
        }
        return View(pet);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _petRepo.DeleteAsync(id);
        return RedirectToAction("Index");
    }

}
