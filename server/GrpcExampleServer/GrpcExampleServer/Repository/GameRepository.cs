using GrpcExampleServer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrpcExampleServer.Repository
{
    public interface IGameRepository
    {
        Task<Game> CreateGame(Game game);
        Task<Game> GetGame(int id);
        Task<List<Game>> GetGames();
    }

    public class GameRepository : IGameRepository
    {
        private readonly Dictionary<int, Game> Storage;
        private object LockKey = new object();
        private int InternalId = 0;
        private int GameId
        {
            get
            {
                lock (LockKey)
                {
                    return InternalId++;
                }
            }
        }

        public GameRepository()
        {
            Storage = new Dictionary<int, Game>();
        }

        public Task<Game> CreateGame(Game game)
        {
            game.Id = GameId;
            Storage.Add(game.Id, game);
            return Task.FromResult(game);
        }

        public Task<Game> GetGame(int id)
        {
            bool ok = Storage.TryGetValue(id, out Game game);
            if (ok)
            {
                return Task.FromResult(game);
            }
            return Task.FromResult<Game>(null);
        }

        public Task<List<Game>> GetGames()
        {
            return Task.FromResult(new List<Game>(Storage.Values));
        }
    }
}
