Feature: CatalogCRUD

Scenario: Create - should succeed
	When I create new catalog with values: name-'firstCatalog' description-'first catalog description'
	Then I see catalog values: name-'firstCatalog' description-'first catalog description' 
	
Scenario: Create - should fail - when name already exists
	When I create new catalog with values: name-'favouriteName' description-'desc'
		
	When I create new catalog with values to fail: name-'favouriteName' description-'desc'
	Then I see catalog last response is status-'Conflict'

Scenario: AddProductToCatalog - should succeed
	When I create new catalog with values: name-'firstCatalog' description-'first catalog description'
	
	When I add product to catalog-'firstCatalog' name-'product' description-'desc' price-'10'
	Then I see catalog product values: name-'product' description-'desc' price-'10'
	
	When I retrieve catalog: name-'firstCatalog'
	Then I see catalog values: name-'firstCatalog' description-'first catalog description' 
	And I see catalog product list values: name-'product' description-'desc' price-'10'

Scenario: AddProductToCatalog - should fail - when catalog not found
	When I add product to fail to catalog-'notExistCatalog' name-'product' description-'desc' price-'10'
	Then I see catalog last response is status-'NotFound'

Scenario: Update - should succeed
	When I create new catalog with values: name-'catalog1' description-'description1'
	
	When I set catalog mandatory values: name-'catalog2' description-'description2'
	And I update this catalog
	Then I see catalog values: name-'catalog2' description-'description2' 
	
Scenario: Update - should fail - when name already exists
	When I create new catalog with values: name-'catalog1' description-'description1'
	When I create new catalog with values: name-'catalog2' description-'description2'

	When I set catalog mandatory values: name-'catalog1' description-'description1'
	And I update this catalog to fail
	Then I see catalog last response is status-'Conflict' 	

Scenario: Update - should fail - when catalog not found
	When I start creating new catalog
	When I set catalog mandatory values: name-'catalog' description-'description'
	And I update this catalog to fail
	Then I see catalog last response is status-'NotFound'
	
Scenario: Delete - should succeed
	When I create new catalog with values: name-'myCatalog' description-'myCatalog description'
	
	When I delete catalog name-'myCatalog' 
	Then I see catalog last response is status-'NoContent'
	
Scenario: Delete - should fail - when catalog does not exist
	When I delete catalog name-'myCatalog' 
	Then I see catalog last response is status-'NotFound'
	
Scenario: Get - should succeed
	When I create new catalog with values: name-'firstCatalog' description-'first catalog description'
	
	When I retrieve catalog: name-'firstCatalog'
	Then I see catalog values: name-'firstCatalog' description-'first catalog description' 
	
Scenario: Get - should fail - when catalog does not exist
	When I retrieve catalog to fail: name-'firstCatalog'
	Then I see catalog last response is status-'NotFound' 

Scenario: GetAll - should succeed
	When I create new catalog with values: name-'firstCatalog1' description-'first catalog1 description'
	When I create new catalog with values: name-'firstCatalog2' description-'first catalog2 description'
	
	When I retrieve all catalogs
	Then I see items in the list count-'2'
	Then I see catalog list contains catalog: name-'firstCatalog1' description-'first catalog1 description'
	Then I see catalog list contains catalog: name-'firstCatalog2' description-'first catalog2 description' 

