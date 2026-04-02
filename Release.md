# Release 2.0.0

### Visible changes:
* **WARNING! Update of the application is not possible. It's necessary to manually uninstall the old version of the application.**
* Signing key of the application changed => necessary to uninstall the old version.
* Questions in the database updated.
* Start button text changed.

### Code changes:
* Correct naming of repository classes.
* Questions generator moved to the application layer.
* Proper naming of the presentation layer.
* Questionnaire body separated to components.
* Removed PageNumber property from the Question class.
* Removed page_number column from the questions table in the database.
* Code fixes - indentation, unnecessary stuff cleanup.

# Release 1.1.0

### Visible changes:
* Bigger, center aligned text on loading screens.
* Color switches instead of checkboxes on the Start screen.
* No category selected by default.
* Start button moved to the bottom part of the screen.
* Empty right padding is visible even on narrow displays when scrolling to the right.
* Fixed faulty texts of some answers.
* Restart button is fully visible even for long questions and scrolling down.

### Code changes:
* GUI stuff extracted to Shared project, potentially reusable in a web application.
* Removed direct reference from Question to Category, both in the code and in the database. Not used, not needed.
* Database interaction handled by entity services.
* Using ISet instead of HashSet for subcategory filtering.
* Distributed apk file size reduced.
