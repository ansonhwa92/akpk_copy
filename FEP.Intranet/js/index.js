SurveyCreator
    .StylesManager
    .applyTheme("bootstrap");

var creatorOptions = {
 // show the embeded survey tab. It is hidden by default
 showEmbededSurveyTab : true,
 // show the "Options" button menu. It is hidden by default 
 showOptions: true                          
};
var surveyCreator = new SurveyCreator.SurveyCreator("creatorElement", creatorOptions);

