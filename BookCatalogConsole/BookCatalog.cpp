#include "pch.h"


namespace BookCatalog
{
    // The catalog of books
    std::vector<Book> books{};

    // The indices of the books that were found whose titles match the search term
    std::vector<std::size_t> found{};

    //
    // Build a list of books from the supplied directory
    //
    void BuildCatalog(const std::filesystem::path& p)
    {
        std::cout << "Building catalog...\n";
        std::size_t count = 0;

        for (const auto& entry : std::filesystem::recursive_directory_iterator(p.string()))
        {
            try
            {
                if (!entry.is_directory())
                {
                    books.emplace_back(Book(entry.path()));
                    if (count % 100 == 0)
                        std::cout << '.';
                    ++count;
                }
            }
            catch (const std::exception& e)
            {
                std::cout << "\nCannot add: " << entry.path().string() << '\n' << e.what() << '\n';
            }
        }

        std::cout << "\nBook catalog contains: " << books.size() << " books\n";
    }

    //
    // Find all book titles containing the search term
    // 
    std::size_t FindAll(std::string term)
    {
        if (term.size() == 1)   // Search for 'R '
            term.append(" ");

        found.clear();

        const std::size_t size = books.size();
        for(std::size_t i = 0; i < size; ++i)
        {
            const Book& book = books[i];
            const std::string& title = book.Title();
            if (title.find(term) != std::string::npos)
                found.emplace_back(i);
        }

        return found.size();
    }

    //
    // Class to add a separator before and after some output
    //
    struct Output final
    {
        explicit Output(const std::string& description)
            : m_size(2 * m_separator.length() + description.length())
        {
            std::cout << m_separator << description << m_separator << '\n';
        }

        ~Output()
        {
            std::cout << std::string(m_size, '=') << '\n';
        }

    private:
        std::string m_separator{ "====================" };
        std::size_t m_size{ 0 };
    };

    //
    // Get the string to search for (title, author, )
    //
    std::string GetSearchTerm()
    {
        std::cout << "Find> ";
        std::string name;
        std::getline(std::cin, name);
        return name;
    }

    //
    // Get the index of the book
    // 
    std::size_t GetIndex(const std::string& prompt)
    {
        std::cout << prompt << "> ";
        std::string data;
        std::getline(std::cin, data);
        const std::size_t val = std::stoi(data);
        return val;
    }

    //
    // Menu functions
    //

    //
    // Perform a search
    //
    bool OnSearch(const std::string& prompt)
    {
        Output o(prompt);

        auto term = GetSearchTerm();

        if (!term.empty())
        {
            /*
            auto pred = [&term](const Book& book) {
                const std::string& title = book.Title();
                return title.find(term) != std::string::npos;
                };

            auto result = books | std::ranges::views::filter(pred);
            */

            std::size_t result = FindAll(term);
            if(result == 0)
            {
                std::cout << "No books found matching the search term " << term << ". Try again.\n";
            }
            else 
            {
                std::size_t count = 0;
                for (auto index : found)
                {
                    const Book& book = books[index];
                    std::cout << "[" << count << "] " << book.Title() << '\n';
                    ++count;
                }
            }
        }
        return true;
    }

    bool OnOpen(const std::string& prompt)
    {
        Output o(prompt);

        auto index = GetIndex("Index");

        if (index >= 0 && index < found.size())
        {
            const Book& book = books[found[index]];
            const std::string& filename = book.Filename();

            ShellExecute(Utils::GetConsoleHwnd(), 
                L"open", 
                std::wstring(begin(filename), end(filename)).c_str(), 
                NULL, 
                NULL, 
                0);
        }

        return true;
    }

    //
    // Exit from the console application
    // 
    bool OnClose(const std::string& prompt)
    {
        std::cout << prompt << '\n';
        return false;
    }

    //
    // Initialise menu items
    //
    MenuItems mainMenu =
    {
        {"1", {"Search (Case sensitive)", OnSearch } },
        {"2", {"Open", OnOpen } },
        {"3", {"Quit", OnClose } }
    };
}

//
// Main entry point
//
int main(int argc, char* argv[])
{
    using namespace BookCatalog;

    auto exit_code = EXIT_FAILURE;

    if (argc < 2) 
    {
        std::cout << "Usage: " << argv[0] << " <PathToBooks>" << '\n';
    }
    else
    {
        std::filesystem::path path(argv[1]);
        if (path.empty()) {
            std::cout << "The <PathToBooks> argument is empty." << '\n';
        }
        else {

            BuildCatalog(path);

            const MenuManager& theMenuManager = MenuManager::Instance(mainMenu);

            for (;;)
            {
                theMenuManager.PrintMenu();
                if (!theMenuManager.ProcessSelection())
                    break;
            }
        }

        exit_code = EXIT_SUCCESS;
    }

    return exit_code;
}
