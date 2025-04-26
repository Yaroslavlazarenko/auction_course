using SothbeysKillerApi.Controllers;
using SothbeysKillerApi.Exceptions;
using SothbeysKillerApi.Entities;
using SothbeysKillerApi.Repository;

namespace SothbeysKillerApi.Services;

public interface IBidService
{
    List<BidResponse> GetBidsByLotId(Guid lotId);
    BidResponse CreateBid(CreateBidRequest request);
}

public class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    private readonly ILotRepository _lotRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IUserRepository _userRepository;

    public BidService(IBidRepository bidRepository, ILotRepository lotRepository, IAuctionRepository auctionRepository, IUserRepository userRepository)
    {
        _bidRepository = bidRepository;
        _lotRepository = lotRepository;
        _auctionRepository = auctionRepository;
        _userRepository = userRepository;
    }

    public List<BidResponse> GetBidsByLotId(Guid lotId)
    {
        var lot = _lotRepository.GetById(lotId);
        
        if (lot is null)
        {
            throw new BidValidationException([new ValidationError("Field", "Некоректні дані для ставки")]);
        }

        //Поки для перевірки отримання закоментовано
        // var auction = _auctionRepository.GetById(lot.AuctionId);
        //
        // if (auction is null)
        // {
        //     throw new BidValidationException(new[] { new ValidationError("Field", "Некоректні дані для ставки") });
        // }
        //
        // if (auction.Start >= DateTime.UtcNow)
        // {
        //     throw new BidValidationException(new[] { new ValidationError("Field", "Некоректні дані для ставки") });
        // }

        var bids = _bidRepository.GetByLotId(lotId).ToList();

        if (bids.Count == 0)
        {
            return new List<BidResponse>();
        }

        var bidResponses = bids
            .Select(bid =>
            {
                var user = _userRepository.GetById(bid.UserId);
                return new BidResponse(user!.Name, bid.Amount, bid.Created);
            })
            .ToList();

        return bidResponses;
    }

    public BidResponse CreateBid(CreateBidRequest request)
    {
        var lot = _lotRepository.GetById(request.LotId);
        
        if (lot is null)
        {
            throw new BidValidationException([new ValidationError("Field", "Некоректні дані для ставки")]);
        }
        
        //Поки для перевірки створення закоментовано
        
        // var auction = _auctionRepository.GetById(lot.AuctionId);
        //
        // if (auction is null)
        // {
        //     throw new BidValidationException(new[] { new ValidationError("Field", "Некоректні дані для ставки") });
        // }
        //
        // if (auction.Start >= DateTime.UtcNow)
        // {
        //     throw new BidValidationException(new[] { new ValidationError("Field", "Некоректні дані для ставки") });
        // }

        var user = _userRepository.GetById(request.UserId);

        if (user is null)
        {
            throw new BidValidationException([new ValidationError("UserId", "Користувача із вказаним ідентифікатором не знайдено")]);
        }

        var lastBidPrice = _bidRepository.GetByLotId(request.LotId)
            .Select(bid => bid.Amount)
            .DefaultIfEmpty(-1)
            .Max();

        if (lastBidPrice == -1)
        {
            if (request.Amount < lot.StartPrice)
            {
                throw new BidValidationException([new ValidationError("Amount", $"Сума ставки має бути не меншою за стартову ціну лота: {lot.StartPrice}")]);
            }
            
        }
        else
        {
            if (request.Amount < lastBidPrice + lot.PriceStep){
                throw new BidValidationException([new ValidationError("Amount", $"Сума ставки має бути не меншою за попередню ставку плюс крок: {lastBidPrice + lot.PriceStep}")]);
            }
        }

        var bid = new Bid()
        {
            Id = Guid.NewGuid(),
            LotId = request.LotId,
            Amount = request.Amount,
            UserId = request.UserId,
            Created = DateTime.UtcNow
        };

        _bidRepository.Create(bid);

        return new BidResponse(user.Name, request.Amount, bid.Created);
    }
}