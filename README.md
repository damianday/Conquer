# Mir 3D EMU Holy Cow

[Latest Build]()

## How to start

### Client & Launcher Configuration

First, a compatible client will need to be downloaded, for example:
* LATEST  [Client](https://mirfiles.com/resources/mir2/users/Jev/Mir%203DEMU/Clients/HC%20-%201.0.4.24.rar)

* If you wish to use the Chinese Pak Files you can download these [here](https://mirfiles.com/resources/mir2/users/Jev/Mir%203DEMU/Clients/HC%20-%201.0.4.24%20CN%20Paks.rar)

Latest version still compatible with this files **[v1.72 - 192912)]**

Once downloaded, you will need to copy the compiled binaries of the launcher to the root of the folder.

In launcher there is a configuration file called "!Settings.txt". In this file we must put our WAN IP and the AccountServer port (by default 7000), eg:

If it is locally: `127.0.0.1:`
If it is WAN: `<public_ip>`

### Account Server Configuration

We will have to create a file in the root of the account server called "!ServerInfo" in which we will include a list of the GS that we will have raised, by default we will only have one, so we will have something like the following:

<details>
  <summary>If it is locally:</summary>

[
  {
  
    "ServerName": "LOMCN",
  
    "TicketAddressIP": "127.0.0.1",
    
    "TicketAddressPort": 6678,
    
    "PublicAddressIP": "127.0.0.1",
    
    "PublicAddressPort": 8701
  }
]

  </details>
  
  <details>
  <summary>If it is WAN:</summary>

[
  {
    
    "ServerName": "LOMCN",
    
    "TicketAddressIP": "127.0.0.1",
    
    "TicketAddressPort": 6678,
    
    "PublicAddressIP": "public_ip",
    
    "PublicAddressPort": 8701
  }
]

  </details>

### Game Server Configuration

We must copy a valid system database in the "Database/System" folder.

It does not require changing the default config.

To publish on the internet, you must open ports 7000 and 8701 on your router

### Network Communication Diagram

![Mir Network](https://cdn.discordapp.com/attachments/1145747473550290944/1233855973941051464/mir-network.png?ex=662e9d6c&is=662d4bec&hm=c771cd7ddf48614e7778266f1eba301a6f8dd59fa2243294da2aba642ce95bcf&)
