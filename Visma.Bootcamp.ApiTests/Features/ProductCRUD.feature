Feature: ProductCRUD
	
Scenario: Get - should succeed
	Given I create new catalog with values: name-'firstCatalog' description-'first catalog description'
	And I add product to catalog-'firstCatalog' name-'product' description-'desc' price-'10'
	
	When I retrieve product: name-'product' 
	Then I see product values: name-'product' description-'desc' price-'10'

# TODO: create this test, need to use existing steps
Scenario: Get - should fail - when product does not exist
	
# TODO: create this test, need to add some steps in ProductSteps
Scenario: Update - should succeed

# TODO: create this test, need to add some steps in ProductSteps
Scenario: Update - should fail - when name already exists

# TODO: create this test, need to add some steps in ProductSteps
Scenario: Update - should fail - when product not found

# TODO: create this test, need to add some steps in ProductSteps and ProductActions
Scenario: Delete - should succeed

# TODO: create this test, need to add some steps in ProductSteps and ProductActions
Scenario: Delete - should fail - when product does not exist

