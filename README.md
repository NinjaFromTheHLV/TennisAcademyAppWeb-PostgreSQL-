# TennisAcademyApp 🎾

Welcome to my first web application – **TennisAcademy**. This platform provides separate, fully functional interfaces for **Users** and **Admins** to manage a professional tennis club ecosystem.

---

## ✨ Features

### 📅 Reservations & Practice
* **Smart Court Reservations:** Book a training session within academy hours with input validations.
* **Live Point Tracking:** Making a reservation rewards the user with ranking points, which are safely locked upon successful completion (`IsCompleted`).
* **Interactive Modals:** View reservation details instantly via sleek Bootstrap popups directly from the index panel.
* **History Log:** Separate view tracking both completed and canceled sessions (points are automatically deducted if canceled by the user).
* *Note: Admins and Coaches do not have reservation privileges.*

### 🛒 Tennis Academy Shop & Ranking Discounts
* **E-Commerce Flow:** Browse rackets, balls, and bags with real-time stock limits.
* **Persistent Cart Management:** Add items multiple times, remove entries, and purchase via Checkout.
* **Leaderboard Rewards:** The top 3 users in the active ranking system automatically receive **exclusive discounts** ($20\%$, $15\%$, or $10\%$) applied dynamically to their shopping carts.
* *All transaction prices are processed in Euro (€).*

### 🏆 Leaderboard & Ranking System
* **Global Standings:** A dynamic ranking list that aggregates user activity across tournaments, reservations, and purchases.
* **Role Filtering:** Automatically excludes system administrators and coaching staff to ensure a fair competition among clients.

### 👥 Coach & Tournament Discovery
* **Professional Staff Search:** Search, filter, and view coach profiles utilizing an elegant UI with pagination.
* **Upcoming Tournaments:** Browse active regional tournaments categorized by skill level with entry fees listed in Euro (€).
* **Favorites List:** Users can add or remove coaches to/from their personal favorites list.

### 🛡️ Administrative Management (Admin-Sided)
* **Shop & Coach Control:** Full CRUD operations to add, edit, or delete inventory and coaching staff.
* **User & Role Administration:** View all registered members, promote/demote user roles (User, Coach, Admin), or terminate accounts.

---

## 🛠️ Technologies Used

### **Backend**
* **ASP.NET Core 8.0 MVC** (Model-View-Controller architecture)
* **Entity Framework Core** (Data access & ORM)
* **ASP.NET Core Identity** (Authentication & Role-based Authorization)

### **Frontend**
* **Razor Views** (Dynamic HTML rendering)
* **Bootstrap 5** & **Bootstrap Icons** (Responsive modern styling)
* **Custom CSS & JavaScript** (Interactive modals and UI animations)

### **Database & Tooling**
* **MS SQL Server** (Data storage)
* **SQL Server Management Studio (SSMS)** (Database management)

### **Testing**
* **NUnit** (Comprehensive unit testing framework)
* **Moq** (Mocking dependencies for isolated logic tests)

---

## 🚀 How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/NinjaFromTheHLV/TennisAcademyApp.git](https://github.com/NinjaFromTheHLV/TennisAcademyApp.git)
