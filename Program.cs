using System;
using System.Collections.Generic;

// Interface
public interface IRepository
{
    void Create(BankAccount account);
    BankAccount Read(string accountNumber);
    void Update(BankAccount account);
    void Delete(string accountNumber);
}

// Abstract Base Class
public abstract class AccountBase
{
    public string AccountNumber { get; set; }
    public string AccountHolder { get; set; }
    public double Balance { get; set; }

    // Abstract method
    public abstract double CalculateInterest();

    // Virtual methods
    public virtual void GenerateStatement()
    {
        Console.WriteLine("Generating generic account statement...");
    }

    public virtual void GenerateStatement(DateTime startDate)
    {
        Console.WriteLine($"Generating statement starting from {startDate.ToShortDateString()}");
    }
}

// Static Validator Class
public static class BankValidator
{
    public static void ValidateString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new FormatException();
        }
    }
}

// Bank Manager (Part 1)
public partial class BankManager : IRepository
{
    // ✅ CHANGED: Array → List
    private List<BankAccount> _accounts = new List<BankAccount>();

    public void Create(BankAccount account)
    {
        if (account == null)
        {
            throw new Exception();
        }

        _accounts.Add(account);
    }
}

// Bank Manager (Part 2)
public partial class BankManager
{
    public BankAccount Read(string accountNumber)
    {
        foreach (var account in _accounts)
        {
            if (account.AccountNumber == accountNumber)
            {
                return account;
            }
        }
        throw new Exception();
    }

    public void Update(BankAccount account)
    {
        BankAccount existing = Read(account.AccountNumber);
        existing.AccountHolder = account.AccountHolder;
        existing.Balance = account.Balance;
    }

    public void Delete(string accountNumber)
    {
        BankAccount accountToRemove = null;

        foreach (var account in _accounts)
        {
            if (account.AccountNumber == accountNumber)
            {
                accountToRemove = account;
                break;
            }
        }

        if (accountToRemove == null)
        {
            throw new Exception();
        }

        _accounts.Remove(accountToRemove);
    }
}

// Derived Class
public class BankAccount : AccountBase
{
    public double InterestRate { get; set; }

    public override double CalculateInterest()
    {
        return Balance * (InterestRate / 100);
    }

    public override void GenerateStatement()
    {
        Console.WriteLine($"Account: {AccountNumber} | Holder: {AccountHolder} | Balance: R{Balance}");
    }
}

internal class NewBaseType
{
    static void Main(string[] args)
    {
        BankManager manager = new BankManager();

        try
        {
            Console.WriteLine("--- Banking Management System ---");

            Console.Write("Enter Account Number: ");
            string accNum = Console.ReadLine();
            BankValidator.ValidateString(accNum);

            Console.Write("Enter Account Holder: ");
            string holder = Console.ReadLine();
            BankValidator.ValidateString(holder);

            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accNum,
                AccountHolder = holder,
                Balance = 7500.50,
                InterestRate = 5.25
            };

            manager.Create(newAccount);

            double interest = newAccount.CalculateInterest();
            Console.WriteLine($"\nRecord Stored. Projected Interest: R{interest}");

            newAccount.GenerateStatement();
            newAccount.GenerateStatement(DateTime.Now);

            Console.WriteLine("\nTesting Delete Operation...");
            manager.Delete(accNum);
            Console.WriteLine("Account removed successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Format not recognized.");
        }
        catch (Exception)
        {
            Console.WriteLine("Error: Operation failed.");
        }
    }
}

