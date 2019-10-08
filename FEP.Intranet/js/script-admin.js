(function($) {
    "use strict";
    var wind = $(window);
    $(function() {
        var akpk = {
            backtotop: function() {
                // Back to top button
                $(window).scroll(function() {
                    if ($(this).scrollTop() > 100) {
                        $('.back-to-top').fadeIn('slow');
                    } else {
                        $('.back-to-top').fadeOut('slow');
                    }
                });
                $('.back-to-top').click(function() {
                    $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
                    return false;
                });

            },

            toggleMainMenu: function() {
                $("#menu-collapse").on('show.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").addClass("open");
                    $(".header__content").addClass('has-border');
                    $(".header-container").css({
                        'height': '128',
                        'transition': 'all 0.4s ease-out 0s'
                    });

                }).on('hide.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").removeClass("open");
                    $(".header__content").removeClass('has-border');
                    $(".header-container").css({
                        'height': '64',
                        'transition': 'all 0.4s ease-in 0s'
                    });
                });

                if (wind.width() >= 991) {
                    $('#menu-collapse').collapse('show');
                }

            },

            //configure the select2 custom plugin
            selectOption: function() {
                $('.select2').select2({
                    minimumResultsForSearch: Infinity
                });

                $('.select2-area').select2({
                    placeholder: 'Select Area'
                });

                //Multiple select
                $('.js-example-basic-multiple').select2({
                    placeholder: 'Select an option'
                });

                //trigger the value of multiple select
                // $('.js-example-basic-multiple').on('select2:selecting select2:unselecting', function(e) {
                //     var value = e.params.args.data.id;
                //     alert(value);
                // });

            },

            dateSelect: function() {
                $(".dateSelect").flatpickr({
                    // mode: "range",
                    dateFormat: "d/m/Y",
                    wrap: true
                });

                $(".timeSelect").flatpickr({
                    enableTime: true,
                    noCalendar: true,
                    dateFormat: "h:iK",
                    static: true
                });
            },

            //for add more event objective
            addButton: function() {
                var eventObjectivehtml = $("#eventobjective").html();
                var btnAddMore = $('#btnAddMore');

                btnAddMore.click(function() {
                    $("#eventobjective").append(eventObjectivehtml);
                });

                $('#eventobjective').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //for add more external exhibitors
            addEe: function() {
                var externalExhibitorshtml = $("#externalexhibitors").html();
                var btnAddEe = $('#btnAddExhibitor');

                btnAddEe.click(function() {
                    $("#externalexhibitors").append(externalExhibitorshtml);
                });

                $('#externalexhibitors').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //for add more add speaker
            addSpeaker: function() {
                var speakerhtml = $("#addspeaker").html();
                var btnAddSpeaker = $('#btnAddspeaker');

                btnAddSpeaker.click(function() {
                    $("#addspeaker").append(speakerhtml);
                });

                $('#addspeaker').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            addLanguage: function() {
                var languagehtml = $("#addlanguage").html();
                var btnAddLanguage = $('#btnAddlanguage');

                btnAddLanguage.click(function() {
                    $("#addlanguage").append(languagehtml);
                });

                $("#addlanguage .lineItem").remove();
                $('#addlanguage').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //list file browse input
            updateList: function() {
                var input = document.getElementById('inputfile');
                var output = document.getElementById('fileList');

                $('#inputfile').change(function() {
                    output.innerHTML = '<ul>';
                    for (var i = 0; i < input.files.length; ++i) {
                        output.innerHTML += '<li>' + input.files.item(i).name + '</li>';
                    }
                    output.innerHTML += '</ul>';
                    $('#fileList').wrapAll("<div class='list-file-container'></div>");
                    $(".list-file-container").find("li").append('<span class="deleteItem"><i class="la la-close color-darkblue font-weight-bolder"></i></span>');
                    $('.list-file-container').on("click", ".deleteItem", function() {
                        $(this).closest("li").remove();
                        // console.log($(this));
                        return;
                    });

                });


            },

            filter: function() {
                $('body').on('click', '.filter.dropdown .cycle-element', function(e) {
                    $(this).parent().toggleClass('open');
                });
                $('body').on('click', function(e) {
                    if (!$('.filter.dropdown').is(e.target) && $('.filter.dropdown').has(e.target).length === 0 && $('.open').has(e.target).length === 0) {
                        $('.filter.dropdown').removeClass('open');
                    }
                });
            },

            //select event category list
            selectCategoryEvent: function() {
                var boxEvent = $("#selecteventcategory .card");

                boxEvent.click(function() {
                    $("#selecteventcategory .box-contain .card").removeClass('active');
                    $(this).addClass('active');
                });

            },

            //For height Action Log Media Interview to trigger scroll
            actionLogHeight: function() {
                var formContainerH = $(".form-container").innerHeight();
                //alert (formContainer);
                $(".action-log .ps").css({
                    'max-height': formContainerH
                });


            },

            showhidebutton: function() {
                $(".showcontent").on('show.bs.collapse', function() {
                    $(this).find("a.btn").removeClass("color-secondary btn-default").addClass("color-white btn-secondary");
                    $(this).find("a.btn").text(function() {
                        //change text based on condition
                        return "HIDE";
                    });

                    $(this).find("a.btn").append('<i class="la la-comment-o color-white pl-1"></i>');
                }).on('hide.bs.collapse', function() {
                    $(this).find("a.btn").removeClass("color-white btn-secondary").addClass("color-secondary btn-default");
                    $(this).find("a.btn").text(function() {
                        //change text based on condition
                        return "ADD";
                    });

                    $(this).find("a.btn").append('<i class="la la-comment-o color-secondary pl-1"></i>');
                });

            },

            //View display
            viewhidebutton: function() {
                $("#ess-details .showcontent").on('show.bs.collapse', function() {
                    $(this).find("a.btn").removeClass("color-secondary btn-default").addClass("color-white btn-secondary");
                    $(this).find("a.btn").text(function() {
                        //change text based on condition
                        return "HIDE";
                    });
                    $(this).find("a.btn").append('<i class="la la-comment-o color-white pl-1"></i>');
                }).on('hide.bs.collapse', function() {
                    $(this).find("a.btn").removeClass("color-white btn-secondary").addClass("color-secondary btn-default");
                    $(this).find("a.btn").text(function() {
                        //change text based on condition
                        return "VIEW";
                    });
                    $(this).find("a.btn").append('<i class="la la-comment-o color-secondary pl-1"></i>');
                });

            },

            //expand
            expandhidebutton: function() {
                $(".showcontent").on('show.bs.collapse', function() {
                    $(this).find("a.btn-expand").removeClass("color-secondary btn-default").addClass("color-white btn-secondary");
                    $(this).find("a.btn-expand").text(function() {
                        //change text based on condition
                        return "HIDE";
                    });

                }).on('hide.bs.collapse', function() {
                    $(this).find("a.btn-expand").removeClass("color-white btn-secondary").addClass("color-secondary btn-default");
                    $(this).find("a.btn-expand").text(function() {
                        //change text based on condition
                        return "EXPAND";
                    });

                });

            },

            mediaSocialShare: function() {
                $(".ms-share").jsSocials({
                    showLabel: false,
                    showCount: false,
                    shares: ["facebook", "twitter", "linkedin", "email"]
                });
            },

            speakersSlide: function() {
                $(".speakers-slider").owlCarousel({
                    items: 3,
                    loop: true,
                    autoplay: true,
                    responsive: {
                        0: {
                            items: 1
                        },
                        576: {
                            items: 1
                        },
                        768: {
                            items: 2
                        },
                        992: {
                            items: 2
                        },
                        1200: {
                            items: 3
                        }
                    }
                });

            },

            //anchor link animation
            anchorAnimation: function() {
                $("a.scrollLink").click(function(event) {
                    event.preventDefault();
                    $("a.scrollLink").removeClass("active");
                    $(this).addClass("active");
                    $("html, body").animate({ scrollTop: $($(this).attr("href")).offset().top }, 500);
                });

            },

            //anchorpreventdefault page nav
            noanchor: function() {
                $(".noanchor.page-nav .nav-item a").click(function(event) {
                    event.preventDefault();

                });

            },

            //clear the input text
            inputClear: function() {
                $(".clearable").each(function() {

                    var $inp = $(this).find("input:text"),
                        $cle = $(this).find(".clearable__clear");

                    $inp.on("input", function() {
                        $cle.toggle(!!this.value);
                    });

                    $cle.on("touchstart click", function(e) {
                        e.preventDefault();
                        $inp.val("").trigger("input");
                    });

                });

            }





        };


        akpk.backtotop();
        akpk.toggleMainMenu();
        akpk.selectOption();
        akpk.dateSelect();
        akpk.addButton();
        akpk.addEe();
        akpk.addSpeaker();
        akpk.addLanguage();
        akpk.updateList();
        akpk.filter();
        akpk.selectCategoryEvent();
        akpk.actionLogHeight();
        akpk.showhidebutton();
        akpk.viewhidebutton();
        akpk.expandhidebutton();
        akpk.mediaSocialShare();
        akpk.speakersSlide();
        akpk.anchorAnimation();
        akpk.noanchor();
        akpk.inputClear();

    });


})(jQuery);