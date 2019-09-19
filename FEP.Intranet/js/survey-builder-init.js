SurveyCreator
    .StylesManager
    .applyTheme("bootstrap");

var creatorOptions = {
 // hide the embeded survey tab. It is hidden by default
 showEmbededSurveyTab : false,
 // hide the property grid on the right. It is shown by default.
 showPropertyGrid: false,
 // hide the "Options" button menu. It is hidden by default 
 showOptions: false,
 // true means every change triggers a save
 isAutoSave: true,
 // true means show saving state
 showState: true
};
var surveyCreator = new SurveyCreator.SurveyCreator("creatorElement", creatorOptions);

