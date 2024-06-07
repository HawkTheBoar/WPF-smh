# Zaverecny projekt na programovani
Knihovna
Zalozena na postgres databazi
connectionstring je v mainwindow.xaml.cs nevim proc neslo nacitat z appconfigu
# DBHelper
ExecuteQuery(string query)
- vykona sql query zadany uzivatelem a vrati jeho data
Connect()
- pripoji se k databazi
InitDB()
- zalozi potrebne tabulky k fungovani aplikace
# RentBookWindow
- Dialogove okno pro vypujceni knihy
## ShowCustomers()
- vrati tabulku s tlacitky, na ktere jsou vsichni ctenari
## UpdateLabels()
- zpusti se po kliknuti na tlacitko, nastavi jmeno ctenare na label
## Rent_click()
- vypne okno a posle data dale
## Cancel_click()
- vypne okno
# HistoryWindow
- jednoduche okno pro zobrazeni historie knihy nebo ctenare
# BooksWindow
## LoadData()
- nacte data do ScrollView pro prohlizeni a interakci s aplikací
## RentBook()
- po kliknuti na tlacitko otevre dialogove okno a vypujci knihu vybranemu zakaznikovi. zavola LoadData()
## ReturnBook()
- po kliknuti na tlacitko vrati knihu
## DeleteBook()
- po kliknuti na tlacitko odstrani knihu z databaze
## AddBook
- po vyplneni formulare a kliknuti na tlacitko prida zadanou knihu do databaze
## ShowHistory
- otevre okno s historií a nastaví mu hodnoty k jeho funkci
# CustomersWindow
## LoadData()
- Nacte data do Scrollview
## AddCustomer
- po vyplneni formulare a kliknuti na tlacitko prida ctenare do databaze
## ShowCustomers
- Vytvori stackpanel ktery naplni tlacitky a textem pro uziti aplikace
## DeleteCustomer
- Smaze ctenare z databaze
## ShowHistory
- Otevre historii s prislusnymi hodnoty
