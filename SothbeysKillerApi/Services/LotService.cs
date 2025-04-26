using Microsoft.AspNetCore.Mvc;
using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Repository;

namespace SothbeysKillerApi.Services;

public interface ILotService
{
    LotResponse LotInfoById(Guid lotId);
    List<LotResponse> GetLotsByAuctionId(Guid auctionId);
    Guid CreateLot(CreateLotRequest request);
    void ModifyLotById(Guid lotId, ModifyLotRequest request);
    void DeleteLotById(Guid lotId);
}

public class LotService : ILotService
{
    private readonly ILotRepository _lotRepository;
    private readonly IAuctionRepository _auctionRepository;

    public LotService(ILotRepository lotRepository, IAuctionRepository auctionRepository)
    {
        _lotRepository = lotRepository;
        _auctionRepository = auctionRepository;
    }

    public LotResponse LotInfoById(Guid lotId)
    {
        var lot = _lotRepository.GetById(lotId);
        
        if (lot is null)
        {
            throw new NullReferenceException("No lots found");
        }
            
        return new LotResponse(lot.Id, lot.AuctionId, lot.Title, lot.Description, lot.StartPrice, lot.PriceStep);
    }

    public List<LotResponse> GetLotsByAuctionId(Guid auctionId)
    {
        var lots = _lotRepository.GetByAuctionId(auctionId)
            .Select(lot => new LotResponse(lot.Id, lot.AuctionId, lot.Title, lot.Description, lot.StartPrice, lot.PriceStep))
            .OrderBy(lot => lot.Title)
            .ToList();
        
        if (lots.Count == 0)
        {
            throw new NullReferenceException("No lots found");
        }
            
        return lots;
    }

    public Guid CreateLot(CreateLotRequest request)
    {
        var auction = _auctionRepository.GetById(request.AuctionId);
        
        if (auction is null)
        {
            throw new ArgumentException("No auction found");
        }
        
        if (auction.Start <= DateTime.Now)
        {
            throw new ArgumentException("Auction already started");
        }

        var lot = new Lot()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            StartPrice = request.StartPrice,
            PriceStep = request.PriceStep,
            AuctionId = request.AuctionId
        };
        
        var created = _lotRepository.Create(lot);
        
        return created.Id;
    }

    public void ModifyLotById(Guid lotId, ModifyLotRequest request)
    {
        var selectedLot = _lotRepository.GetById(lotId);
        
        if (selectedLot is null)
        {
            throw new NullReferenceException("No lots found");
        }
            
        var auction = _auctionRepository.GetById(selectedLot.AuctionId);
        
        if (auction is null)
        {
            throw new ArgumentException("No matching auction found.");
        }
        
        if (auction.Start <= DateTime.Now)
        {
            throw new ArgumentException("Auction already started");
        }

        var lot = new Lot()
        {
            Id = selectedLot.Id,
            Title = request.Title,
            Description = request.Description,
            StartPrice = request.StartPrice,
            PriceStep = request.PriceStep,
            AuctionId = selectedLot.AuctionId
        };
        
        _lotRepository.Update(lot);
    }

    public void DeleteLotById(Guid lotId)
    {
        var selectedLot = _lotRepository.GetById(lotId);
        
        if (selectedLot is null)
        {
            throw new NullReferenceException("No lots found");
        }
            
        var auction = _auctionRepository.GetById(selectedLot.AuctionId);
        
        if (auction is null)
        {
            throw new ArgumentException("No matching auction found.");
        }
        
        if (auction.Start <= DateTime.Now)
        {
            throw new ArgumentException("Auction already started");
        }
        
        _lotRepository.Delete(lotId);
    }
}