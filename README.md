# Proxmox Manager

A Windows Forms (.NET 8) desktop application for managing Proxmox VE environments.

## Features
- **Connection Management**: securely store credentials for multiple Proxmox servers.
- **Cluster Sync**: Automatically discovers all nodes, VMs, and Containers in the cluster.
- **Detailed View**: View status and IP addresses for running VMs (via QEMU Guest Agent) and Containers.
- **Secure**: Credentials stored using Windows Credential Manager.

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows OS (due to Windows Forms and Credential Manager dependency)

## How to Run
1. Open the solution in Visual Studio or VS Code.
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run --project ProxmoxManager/ProxmoxManager
   ```

## Project Structure
- `ProxmoxManager/`: Main application project.
- `ProxmoxManager/Services/`: API and authentication logic.
- `ProxmoxManager/ViewModels/`: MVVM logic for UI.
- `ProxmoxManager/Models/`: Data models for Proxmox resources.
