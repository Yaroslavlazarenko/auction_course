using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Exceptions;
using SothbeysKillerApi.Entities;
using SothbeysKillerApi.Repository;

namespace SothbeysKillerApi.Services;

public interface IAuctionService
{
    List<AuctionResponse> GetPastAuctions();
    List<AuctionResponse> GetActiveAuctions();
    List<AuctionResponse> GetFutureAuctions();
    Guid CreateAuction(AuctionCreateRequest request);
    AuctionResponse GetAuctionById(Guid id);
    void UpdateAuction(Guid id, AuctionUpdateRequest request);
    void DeleteAuction(Guid id);
}

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    
    public AuctionService(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }
    
    public List<AuctionResponse> GetPastAuctions()
    {
        var auctions = _auctionRepository.GetPast();

        return auctions
            .Select(auction => new AuctionResponse(auction.Id, auction.Title, auction.Start, auction.Finish))
            .ToList();
    }
    
    public List<AuctionResponse> GetActiveAuctions()
    {
        var auctions = _auctionRepository.GetActive();

        return auctions
            .Select(auction => new AuctionResponse(auction.Id, auction.Title, auction.Start, auction.Finish))
            .ToList();
    }
    
    public List<AuctionResponse> GetFutureAuctions()
    {
        var auctions = _auctionRepository.GetFuture();

        return auctions
            .Select(auction => new AuctionResponse(auction.Id, auction.Title, auction.Start, auction.Finish))
            .ToList();
    }

    public Guid CreateAuction(AuctionCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length < 3 || request.Title.Length > 255)
        {
            throw new AuctionValidationException([new ValidationError("Title", "Назва аукціону повинна містити від 3 до 255 символів")]);
        }
        
        if (request.Start < DateTime.UtcNow)
        {
            throw new AuctionValidationException([new ValidationError("Start", "Дата початку аукціону повинна бути пізніше поточного часу")]);
        }
        
        if (request.Finish <= request.Start)
        {
            throw new AuctionValidationException([new ValidationError("Finish", "Дата завершення повинна бути пізніше дати початку аукціону")]);
        }
        
        var auction = new Auction()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Start = request.Start,
            Finish = request.Finish
        };

        var created = _auctionRepository.Create(auction);

        return created.Id;
    }

    public AuctionResponse GetAuctionById(Guid id)
    {
        var auction = _auctionRepository.GetById(id);

        if (auction is null)
        {
            throw new AuctionValidationException([new ValidationError("Id", "Auction not found")]);
        }
            
        var response = new AuctionResponse(auction.Id, auction.Title, auction.Start, auction.Finish);
            
        return response;
    }

    public void UpdateAuction(Guid id, AuctionUpdateRequest request)
    {
        var auction = _auctionRepository.GetById(id);
        
        if (auction is null)
        {
            throw new AuctionValidationException([new ValidationError("Id", "Auction not found")]);
        }

        if (auction.Start <= DateTime.UtcNow)
        {
            throw new AuctionValidationException([new ValidationError("Start", "Аукціон вже розпочато, редагування неможливе")
            ]);
        }
        
        if (request.Start < DateTime.UtcNow)
        {
            throw new AuctionValidationException([new ValidationError("Start", "Дата початку аукціону повинна бути пізніше поточного часу")
            ]);
        }

        if (request.Finish <= request.Start)
        {
            throw new AuctionValidationException([new ValidationError("Finish", "Дата завершення повинна бути пізніше дати початку аукціону")
            ]);
        }

        auction.Start = request.Start;
        auction.Finish = request.Finish;

        _auctionRepository.Update(auction);
    }

    public void DeleteAuction(Guid id)
    {
        var auction = _auctionRepository.GetById(id);
        
        if (auction is null)
        {
            throw new AuctionValidationException([new ValidationError("Id", "Auction not found")]);
        }
        
        if (auction.Start <= DateTime.UtcNow)
        {
            throw new AuctionValidationException([new ValidationError("Start", "Аукціон вже розпочато, видалення неможливе")]);
        }

        _auctionRepository.Delete(id);
    }
}