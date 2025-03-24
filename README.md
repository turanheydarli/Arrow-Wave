
## Updates & Features
- **Coins System**  
  - Added a `CurrencyManager` to handle coins earned at each level and save them using ES3.  
  - *Reference: [CurrencyManager.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Currencies/CurrencyManager.cs)*

- **Store & Skin Customization**  
  - Implemented a Store UI (button & panel) that lets players purchase arrow and bow skins while preserving the original UI structure.  
  - *Reference: [ShopScreen.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/UI/Screens/ShopPanel/ShopScreen.cs)*
  - Updated inventory management for skins.  
  - *References: [InventoryManager.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/InventoryManager.cs), [BowInventoryItem.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/BowInventoryItem.cs), [ArrowInventoryItem.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Inventory/ArrowInventoryItem.cs)*

## Bug Fixes
- **Arrow Sticking Issue**  
  - Resolved by editing `Arrow.cs`.  
  - *Reference: [Arrow.cs](https://github.com/turanheydarli/Arrow-Wave/blob/main/Assets/Game/Script/Controllers/Elements/Player/Arrow.cs)*

- **Unlimited Arrows**  
  - Implemented a maximum arrow limit on bow skins to prevent unlimited arrows.

- **Arrow Speed Increase Bug**  
  - Issue was not observed during testing.

## Build 
- **APK Download**: [Download APK](https://drive.google.com/file/d/1zed6cAtITX2p2YjfUhneFvl7ok9scmOJ/view?usp=drive_link)

_All scripts include comments indicating the changes made for easier review._
