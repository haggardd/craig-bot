﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CraigBot.Bot.Common;
using CraigBot.Bot.Helpers;
using CraigBot.Core.Models;
using CraigBot.Core.Repositories;
using CraigBot.Core.Services;
using Discord;

namespace CraigBot.Bot.Services
{
    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IBankingService _bankingService;
        
        public BetService(IBetRepository betRepository, IBankingService bankingService)
        {
            _betRepository = betRepository;
            _bankingService = bankingService;
        }
        
        public async Task<IEnumerable<Bet>> GetAllActiveBets()
        {
            var bets = await _betRepository.GetAllBets();

            var activeBets = bets.Where(x => !x.HasEnded);

            return activeBets;
        }

        public async Task<Bet> GetActiveBetById(int id)
        {
            var bet = await _betRepository.GetBetById(id);

            if (bet == null)
            {
                return null;
            }
            
            return bet.HasEnded 
                ? null 
                : bet;
        }

        public async Task<IEnumerable<Wager>> GetWagersByBetId(int id)
        {
            var wagers = await _betRepository.GetWagersByBetId(id);

            return wagers;
        }

        public async Task<Bet> CreateBet(IUser user, string description, string forOdds, string againstOdds)
        {
            var bet = new Bet
            {
                UserId = user.Id,
                Username = user.Username,
                Description = description,
                ForOdds = forOdds,
                AgainstOdds = againstOdds,
                HasEnded = false
            };

            var newBet = await _betRepository.CreateBet(bet);

            return newBet;
        }

        public async Task<Wager> CreateWager(IUser user, int betId, decimal stake, bool inFavour)
        {
            var wager = new Wager
            {
                UserId = user.Id,
                BetId = betId,
                Username = user.Username,
                Stake = stake,
                InFavour = inFavour
            };

            var newWager = await _betRepository.CreateWager(wager);

            return newWager;
        }

        public async Task<BetResult> EndBet(Bet bet, bool result)
        {
            var wagers = (await _betRepository.GetWagersByBetId(bet.Id)).ToList();

            var wagerResults = new List<WagerResult>();

            Fraction.TryParse(bet.ForOdds, out var forOdds);
            Fraction.TryParse(bet.AgainstOdds, out var againstOdds);

            if (wagers.Any())
            {
                foreach (var wager in wagers)
                {
                    var returns = 0.00M;
                    
                    if (result == wager.InFavour)
                    {
                        returns = wager.InFavour
                            ? wager.Stake.CalculateWinnings(forOdds)
                            : wager.Stake.CalculateWinnings(againstOdds);
                    }

                    var bankAccount = await _bankingService.GetAccount(wager.UserId);

                    var wagerResult = new WagerResult
                    {
                        Username = wager.Username,
                        Returns = returns,
                        InFavour = wager.InFavour
                    };
                
                    wagerResults.Add(wagerResult);
                
                    await _bankingService.Deposit(bankAccount, returns);
                }
            }

            var betResult = new BetResult
            {
                Username = bet.Username,
                Description = bet.Description,
                WagerResults = wagerResults
            };
            
            bet.HasEnded = true;

            await _betRepository.UpdateBet(bet);

            return betResult;
        }

        public async Task VoidBet(Bet bet)
        {
            var wagers = (await _betRepository.GetWagersByBetId(bet.Id)).ToList();
            
            if (wagers.Any())
            {
                foreach (var wager in wagers)
                {
                    var bankAccount = await _bankingService.GetAccount(wager.UserId);

                    await _bankingService.Deposit(bankAccount, wager.Stake);
                }
            }
            
            bet.HasEnded = true;

            await _betRepository.UpdateBet(bet);
        }
    }
}