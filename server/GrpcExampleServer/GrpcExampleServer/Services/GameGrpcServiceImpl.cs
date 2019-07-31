using Grpc.Core;
using GrpcExampleServer.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace GrpcExampleServer.Services
{
    public class GameGrpcServiceImpl : Grpc.GameService.GameServiceBase
    {
        private readonly IGameRepository GameRepository;

        public GameGrpcServiceImpl(IGameRepository gameRepository)
        {
            GameRepository = gameRepository;
        }

        public override async Task<Grpc.CreateGameResponse> CreateGame(Grpc.CreateGameRequest request, ServerCallContext context)
        {
            var createdGame = await GameRepository.CreateGame(request.Game.ToDomainModel());
            return new Grpc.CreateGameResponse
            {
                ErrorCode = 0,
                ErrorMessage = string.Empty,
                Game = createdGame.ToGrpcModel()
            };
        }

        public override async Task<Grpc.GetGameResponse> GetGame(Grpc.GetGameRequest request, ServerCallContext context)
        {
            var game = await GameRepository.GetGame(request.Id);
            return new Grpc.GetGameResponse
            {
                ErrorCode = 0,
                ErrorMessage = string.Empty,
                Game = game.ToGrpcModel()
            };
        }

        public override async Task<Grpc.GetGamesResponse> GetGames(Grpc.GetGamesRequest request, ServerCallContext context)
        {
            var games = await GameRepository.GetGames();
            var response = new Grpc.GetGamesResponse
            {
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };
            response.Games.AddRange(games.ToGrpcModel());
            return response;
        }
    }

    static public class GrpcToDomainConverterOfGame
    {
        static public Models.Game ToDomainModel(this Grpc.Game game)
        {
            var domainModel = new Models.Game()
            {
                Id = game.Id,
                GameCode = game.GameCode,
                Name = game.Name,
                Platform = (Models.Platform)game.Platform,
                LandingUrl = game.LandingUrl,
                ImageUrl = game.ImageUrl
            };
            return domainModel;
        }

        static public Grpc.Game ToGrpcModel(this Models.Game game)
        {
            var grpcModel = new Grpc.Game()
            {
                Id = game.Id,
                GameCode = game.GameCode,
                Name = game.Name,
                Platform = (Grpc.Game.Types.Platform)game.Platform,
                LandingUrl = game.LandingUrl,
                ImageUrl = game.ImageUrl
            };
            return grpcModel;
        }

        static public List<Grpc.Game> ToGrpcModel(this List<Models.Game> games)
        {
            var list = new List<Grpc.Game>(games.Select(game => game.ToGrpcModel()));
            return list;
        }
    }
}
