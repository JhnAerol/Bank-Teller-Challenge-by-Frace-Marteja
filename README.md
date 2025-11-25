# Bank Management System - User Guide

## 📋 Project Overview
A console-based banking application that allows tellers to manage customer accounts, process transactions, and view transaction history with a user-friendly menu interface.

## 🏦 System Features

### 🔐 Authentication
- **Username**: `Aerol`
- **PIN**: `12345`

### 💼 Available Operations
1. **Create Account** - Open new customer accounts
2. **Check Balance** - View account balances
3. **Deposit** - Add funds to accounts
4. **Withdraw** - Remove funds from accounts
5. **View Transactions** - See transaction history
6. **List of Accounts** - View all customer accounts
7. **Exit** - Close the application

## 🎮 Navigation Guide

### Login Screen
```
Enter UserName: Aerol
Enter PIN: *****
```
- PIN is hidden with asterisks (`*`) for security
- Press `Enter` after typing PIN

### Main Menu Navigation
- Use **↑ UP ARROW** and **↓ DOWN ARROW** keys to move through options
- Press **ENTER** to select a highlighted option
- Selected option turns **GREEN**

### Transaction Workflows

#### 🆕 Create Account
1. Enter customer name
2. Enter initial deposit amount
3. System validates:
   - Positive deposit amount required
   - Unique account name required
4. Option to create another account after success

#### 💰 Deposit & Withdraw
1. Enter account name
2. Enter transaction amount
3. System validates:
   - Account must exist
   - Positive amount required
   - Sufficient funds for withdrawals
4. Option to perform another transaction after completion

#### 📊 Check Balance
- Enter account name to view current balance
- Returns to main menu after viewing

#### 📋 List of Accounts
- Displays all registered customer names
- Shows "No Records" if no accounts exist

#### 📈 View Transactions
**Two options:**
1. **Teller Transactions** - All system-wide transactions
2. **Customer Transactions** - Specific account history
   - Enter account name to view individual transaction history

## ⚠️ Important Notes

### Input Requirements
- **Names**: Case-insensitive matching
- **Amounts**: Positive decimal numbers only
- **PIN**: Numeric values only

### Error Handling
- Invalid inputs show clear error messages
- System prevents duplicate account names
- Insufficient fund checks for withdrawals
- Input validation for all user entries

### Transaction Records
- All transactions are timestamped
- Both teller and customer perspectives recorded
- Full audit trail maintained

## 🔄 Program Flow
1. **Login** → **Main Menu** → **Select Operation** → **Process Transaction** → **Return to Menu**

### Multi-Transaction Features
- **Create Account**: Can create multiple accounts sequentially
- **Deposit**: Can process multiple deposits without returning to menu
- **Withdraw**: Can process multiple withdrawals without returning to menu

## 🚀 Getting Started
1. Run the application
2. Login with provided credentials
3. Navigate using arrow keys and Enter
4. Follow on-screen prompts for each operation

## 💡 Tips for Use
- Use clear, unique customer names
- Always verify account names before transactions
- Check transaction history for audit purposes
- Use the multi-transaction feature for batch operations

This system provides a complete banking solution with secure authentication, comprehensive transaction tracking, and intuitive navigation for efficient banking operations.
