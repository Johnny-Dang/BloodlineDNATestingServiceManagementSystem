using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories;

public class SurchargePriceRepository
{
    private readonly DnatestingServiceContext _context;

    public SurchargePriceRepository(DnatestingServiceContext context)
    {
        _context = context;
    }

    public IEnumerable<SurchargePrice> GetAll()
    {
        return _context.SurchargePrices.ToList();
    }

    public SurchargePrice? GetById(int id)
    {
        return _context.SurchargePrices.FirstOrDefault(x => x.SurchargeId == id);
    }

    public void Add(SurchargePrice surchargePrice)
    {
        _context.SurchargePrices.Add(surchargePrice);
        _context.SaveChanges();
    }

    public void Update(SurchargePrice surchargePrice)
    {
        _context.SurchargePrices.Update(surchargePrice);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.SurchargePrices.FirstOrDefault(x => x.SurchargeId == id);
        if (entity != null)
        {
            _context.SurchargePrices.Remove(entity);
            _context.SaveChanges();
        }
    }
} 