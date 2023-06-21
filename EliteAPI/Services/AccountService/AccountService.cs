﻿using EliteAPI.Data;
using EliteAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EliteAPI.Services.AccountService;

public class AccountService : IAccountService
{

    private readonly DataContext _context;
    public AccountService(DataContext context)
    {
        _context = context;
    }

    public async Task<Account?> AddAccount(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account;
    }

    public async Task<Account?> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null) return null;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return account ?? null;
    }

    public async Task<Account?> GetAccountByApiKey(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetAccount(int accountId)
    {
        return await _context.Accounts.FindAsync(accountId);
    }

    public async Task<Account?> GetAccountByDiscordId(ulong id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<Account?> GetAccountByIgn(string ign)
    {
        return await _context.Accounts
           .Where(account => account.MinecraftAccounts.Exists(mc => mc.Name.Equals(ign)))
           .FirstOrDefaultAsync();
    }

    public async Task<Account?> GetAccountByMinecraftUuid(string uuid)
    {
        return await _context.Accounts
           .Where(account => account.MinecraftAccounts.Exists(mc => mc.Id.Equals(uuid)))
           .FirstOrDefaultAsync();
    }

    public async Task<Account?> UpdateAccount(int id, Account request)
    {
        //if (request == null) return null;

        var account = await _context.Accounts.FindAsync(id);
        if (account == null) return null;

        // Update account
        // account.MinecraftAccounts = request.MinecraftAccounts;
        // account.DiscordAccount = request.DiscordAccount;
        // account.PremiumUser = request.PremiumUser;

        await _context.SaveChangesAsync();
        return account;
    }
}
