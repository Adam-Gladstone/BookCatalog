# Issues

# Wishlist
- Update the icon: WindowIcon doesn't display well in dark mode. Also the taskbar icon is 'generic'. Generate a complete set of Icons:
https://learn.microsoft.com/en-us/windows/apps/design/style/iconography/visual-studio-asset-generation
NOTE: Free Icons8: <a target="_blank" href="https://icons8.com/icon/ChugQlss1ohB/books">Books</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- Support packaging the app

# Changes
## 07/11/2023
NOTE: Support for creating the file based SQLite database: the original relied on a UWP service in Windows.Storage which is only available for packaged (?) apps (i.e. apps run from Windows Store?). Changed the target platform from net7.0-windows10.0.19.... to net6.0-windows10.0.22000.0 for all csproj files. Then added CommunityToolkit.Mvvm, CommunityToolkit.WinUI.UI.Controls, Microsoft.WindowsAppSDK references. After this, rebuild: now Windows.Storage is available, hence CreateFileAsync is available with the correct flags for creating a sqlite db.

## 28/12/2023
Initial checkin. Supports the following functionality:
- Flyout menu with page selection using tags.
- Add single/multiple book title(s)/item(s) to the database from a directory.
- Use the directory name as a grouping key.
- Delete selected item from the database.
- Display the book titles in a (DevExpress) grid with sorting, searching, filtering and grouping enabled.
- Double-click opens the book title in a browser or associated application.











