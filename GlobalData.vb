Public Class GlobalData
    Public Brands As _Brand() = CreateBrandsArray()
    Public Categories As _Category() = CreateCategoriesArray()
    Public Items As _Item() = CreateItemsArray()


    Public Function CreateItemsArray() As _Item()
        Dim Items As _Item() = {
            New _Item With {.item_id = 0, .item_name = "Mixers", .item_quantity = 50, .item_selling_price = 1200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 1, .item_name = "Roller Mills", .item_quantity = 40, .item_selling_price = 1500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 2, .item_name = "Extruders", .item_quantity = 30, .item_selling_price = 2500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 3, .item_name = "Winding Machines", .item_quantity = 25, .item_selling_price = 3000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 4, .item_name = "Vulcanization Equipment", .item_quantity = 10, .item_selling_price = 3500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 5, .item_name = "Grinders", .item_quantity = 60, .item_selling_price = 1000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 6, .item_name = "Lathe Machines", .item_quantity = 15, .item_selling_price = 5000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 7, .item_name = "Core Shafts", .item_quantity = 50, .item_selling_price = 1200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 8, .item_name = "Pre-heaters", .item_quantity = 40, .item_selling_price = 2500.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 9, .item_name = "Cooling Systems", .item_quantity = 30, .item_selling_price = 2000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 10, .item_name = "CNC Machines", .item_quantity = 20, .item_selling_price = 8000.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 11, .item_name = "Coating Machines", .item_quantity = 45, .item_selling_price = 1800.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 12, .item_name = "Polishing Machines", .item_quantity = 35, .item_selling_price = 2200.0D, .brand_id = 1, .category_id = 1, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 13, .item_name = "Offset Printing Plates", .item_quantity = 50, .item_selling_price = 700.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 14, .item_name = "Flexographic Plates", .item_quantity = 40, .item_selling_price = 750.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 15, .item_name = "Gravure Cylinders", .item_quantity = 30, .item_selling_price = 850.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 16, .item_name = "Letterpress Plates", .item_quantity = 20, .item_selling_price = 600.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 17, .item_name = "Thermographic Plates", .item_quantity = 15, .item_selling_price = 950.0D, .brand_id = 2, .category_id = 2, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 18, .item_name = "Rubber Compounds (natural rubber)", .item_quantity = 100, .item_selling_price = 100.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 19, .item_name = "Rubber Compounds (nitrile)", .item_quantity = 100, .item_selling_price = 120.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 20, .item_name = "Rubber Compounds (EPDM)", .item_quantity = 100, .item_selling_price = 130.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 21, .item_name = "Rubber Compounds (silicone)", .item_quantity = 100, .item_selling_price = 150.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 22, .item_name = "Rubber Compounds (polyurethane)", .item_quantity = 100, .item_selling_price = 180.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 23, .item_name = "Curing Agents (sulfur)", .item_quantity = 200, .item_selling_price = 50.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 24, .item_name = "Curing Agents (peroxides)", .item_quantity = 200, .item_selling_price = 60.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 25, .item_name = "Adhesives", .item_quantity = 150, .item_selling_price = 90.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 26, .item_name = "Solvents and Cleaners", .item_quantity = 180, .item_selling_price = 70.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 27, .item_name = "Anti-static Agents", .item_quantity = 100, .item_selling_price = 40.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now},
            New _Item With {.item_id = 28, .item_name = "Release Agents", .item_quantity = 120, .item_selling_price = 85.0D, .brand_id = 3, .category_id = 3, .created_by = 1, .created_at = DateTime.Now}
            }

        Return Items
    End Function

    Function CreateCategoriesArray() As _Category()
        Dim Categories As _Category() = {
            New _Category With {.category_id = 1, .category_name = "Equipment for Rubber Roller", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 2, .category_name = "Printing Plates", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 3, .category_name = "Chemistry for Rubber Rollers and Printing", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 4, .category_name = "Measuring Devices and Instrumentation", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 5, .category_name = "Rubber Roller Applications in Printing", .created_by = 1, .created_at = DateTime.Now},
            New _Category With {.category_id = 6, .category_name = "Other Products", .created_by = 1, .created_at = DateTime.Now}
        }

        Return Categories
    End Function

    Public Function CreateBrandsArray() As _Brand()
        Dim Brands As _Brand() = {
            New _Brand With {.brand_id = 1, .brand_name = "RubberTech", .created_by = 1, .created_at = DateTime.Now},
            New _Brand With {.brand_id = 2, .brand_name = "PrintTech", .created_by = 1, .created_at = DateTime.Now},
            New _Brand With {.brand_id = 3, .brand_name = "ChemRubber", .created_by = 1, .created_at = DateTime.Now}
        }

        Return Brands
    End Function
End Class