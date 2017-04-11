(function () {
    angular.module('app').directive('artChatEmoticonPicker', artChatEmoticonPickerDirective);

    function artChatEmoticonPickerDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-emoticon-picker/art-chat-emoticon-picker.html',
            scope: {},
            controller: ChatEmoticonPickerDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('ChatEmoticonPickerDirectiveController:', ChatEmoticonPickerDirectiveController);

    ChatEmoticonPickerDirectiveController.$inject = ['$scope', '$element', 'cookieService'];

    function ChatEmoticonPickerDirectiveController($scope, $element, cookieService) {
        var t = this;

        t.recentlyUsed = [];

        t.sections = [{
            title: 'Recent',
            emoticons: t.recentlyUsed
        }, {
            title: 'Smileys',
            emoticons: [':grinning:', ':grimacing:', ':grin:', ':joy:', ':smiley:', ':smile:', ':sweat_smile:', ':laughing:', ':innocent:', ':wink:', ':blush:', ':slight_smile:', ':upside_down:', ':relaxed:', ':yum:', ':relieved:', ':heart_eyes:', ':kissing_heart:', ':kissing:', ':kissing_smiling_eyes:', ':kissing_closed_eyes:', ':stuck_out_tongue_winking_eye:', ':stuck_out_tongue_closed_eyes:', ':stuck_out_tongue:', ':money_mouth:', ':nerd:', ':sunglasses:', ':hugging:', ':smirk:', ':no_mouth:', ':neutral_face:', ':expressionless:', ':unamused:', ':rolling_eyes:', ':thinking:', ':flushed:', ':disappointed:', ':worried:', ':angry:', ':rage:', ':pensive:', ':confused:', ':slight_frown:', ':frowning2:', ':persevere:', ':confounded:', ':tired_face:', ':weary:', ':triumph:', ':open_mouth:', ':scream:', ':fearful:', ':cold_sweat:', ':hushed:', ':frowning:', ':anguished:', ':cry:', ':disappointed_relieved:', ':sleepy:', ':sweat:', ':sob:', ':dizzy_face:', ':astonished:', ':zipper_mouth:', ':mask:', ':thermometer_face:', ':head_bandage:', ':sleeping:', ':zzz:', ':raised_hands:', ':clap:', ':wave:', ':thumbsup:', ':thumbsdown:', ':punch:', ':fist:', ':v:', ':ok_hand:', ':raised_hand:', ':open_hands:', ':muscle:', ':pray:', ':point_up:', ':point_up_2:', ':point_down:', ':point_left:', ':point_right:', ':middle_finger:', ':hand_splayed:', ':metal:']
        }, {
            title: 'Symbols',
            emoticons: [':heart:', ':yellow_heart:', ':green_heart:', ':blue_heart:', ':purple_heart:', ':broken_heart:', ':heart_exclamation:', ':two_hearts:', ':revolving_hearts:', ':heartbeat:', ':heartpulse:', ':sparkling_heart:', ':cupid:', ':gift_heart:', ':heart_decoration:', ':secret:', ':congratulations:', ':exclamation:', ':grey_exclamation:', ':question:', ':grey_question:']
        }, {
            title: 'Flags',
            emoticons: [':flag_ac:', ':flag_ad:', ':flag_ae:', ':flag_af:', ':flag_ag:', ':flag_ai:', ':flag_al:', ':flag_am:', ':flag_ao:', ':flag_aq:', ':flag_ar:', ':flag_as:', ':flag_at:', ':flag_au:', ':flag_aw:', ':flag_ax:', ':flag_az:', ':flag_ba:', ':flag_bb:', ':flag_bd:', ':flag_be:', ':flag_bf:', ':flag_bg:', ':flag_bh:', ':flag_bi:', ':flag_bj:', ':flag_bl:', ':flag_bm:', ':flag_bn:', ':flag_bo:', ':flag_bq:', ':flag_br:', ':flag_bs:', ':flag_bt:', ':flag_bv:', ':flag_bw:', ':flag_by:', ':flag_bz:', ':flag_ca:', ':flag_cc:', ':flag_cd:', ':flag_cf:', ':flag_cg:', ':flag_ch:', ':flag_ci:', ':flag_ck:', ':flag_cl:', ':flag_cm:', ':flag_cn:', ':flag_co:', ':flag_cp:', ':flag_cr:', ':flag_cu:', ':flag_cv:', ':flag_cw:', ':flag_cx:', ':flag_cy:', ':flag_cz:', ':flag_de:', ':flag_dg:', ':flag_dj:', ':flag_dk:', ':flag_dm:', ':flag_do:', ':flag_dz:', ':flag_ea:', ':flag_ec:', ':flag_ee:', ':flag_eg:', ':flag_eh:', ':flag_er:', ':flag_es:', ':flag_et:', ':flag_eu:', ':flag_fi:', ':flag_fj:', ':flag_fk:', ':flag_fm:', ':flag_fo:', ':flag_fr:', ':flag_ga:', ':flag_gb:', ':flag_gd:', ':flag_ge:', ':flag_gf:', ':flag_gg:', ':flag_gh:', ':flag_gi:', ':flag_gl:', ':flag_gm:', ':flag_gn:', ':flag_gp:', ':flag_gq:', ':flag_gr:', ':flag_gs:', ':flag_gt:', ':flag_gu:', ':flag_gw:', ':flag_gy:', ':flag_hk:', ':flag_hm:', ':flag_hn:', ':flag_hr:', ':flag_ht:', ':flag_hu:', ':flag_ic:', ':flag_id:', ':flag_ie:', ':flag_il:', ':flag_im:', ':flag_in:', ':flag_io:', ':flag_iq:', ':flag_ir:', ':flag_is:', ':flag_it:', ':flag_je:', ':flag_jm:', ':flag_jo:', ':flag_jp:', ':flag_ke:', ':flag_kg:', ':flag_kh:', ':flag_ki:', ':flag_km:', ':flag_kn:', ':flag_kp:', ':flag_kr:', ':flag_kw:', ':flag_ky:', ':flag_kz:', ':flag_la:', ':flag_lb:', ':flag_lc:', ':flag_li:', ':flag_lk:', ':flag_lr:', ':flag_ls:', ':flag_lt:', ':flag_lu:', ':flag_lv:', ':flag_ly:', ':flag_ma:', ':flag_mc:', ':flag_md:', ':flag_me:', ':flag_mg:', ':flag_mh:', ':flag_mk:', ':flag_ml:', ':flag_mm:', ':flag_mn:', ':flag_mo:', ':flag_mp:', ':flag_mq:', ':flag_mr:', ':flag_ms:', ':flag_mt:', ':flag_mu:', ':flag_mv:', ':flag_mw:', ':flag_mx:', ':flag_my:', ':flag_mz:', ':flag_na:', ':flag_nc:', ':flag_ne:', ':flag_nf:', ':flag_ng:', ':flag_ni:', ':flag_nl:', ':flag_no:', ':flag_np:', ':flag_nr:', ':flag_nu:', ':flag_nz:', ':flag_om:', ':flag_pa:', ':flag_pe:', ':flag_pf:', ':flag_pg:', ':flag_ph:', ':flag_pk:', ':flag_pl:', ':flag_pm:', ':flag_pn:', ':flag_pr:', ':flag_ps:', ':flag_pt:', ':flag_pw:', ':flag_py:', ':flag_qa:', ':flag_re:', ':flag_ro:', ':flag_rs:', ':flag_ru:', ':flag_rw:', ':flag_sa:', ':flag_sb:', ':flag_sc:', ':flag_sd:', ':flag_se:', ':flag_sg:', ':flag_sh:', ':flag_si:', ':flag_sj:', ':flag_sk:', ':flag_sl:', ':flag_sm:', ':flag_sn:', ':flag_so:', ':flag_sr:', ':flag_ss:', ':flag_st:', ':flag_sv:', ':flag_sx:', ':flag_sy:', ':flag_sz:', ':flag_ta:', ':flag_tc:', ':flag_td:', ':flag_tf:', ':flag_tg:', ':flag_th:', ':flag_tj:', ':flag_tk:', ':flag_tl:', ':flag_tm:', ':flag_tn:', ':flag_to:', ':flag_tr:', ':flag_tt:', ':flag_tw:', ':flag_tz:', ':flag_ua:', ':flag_ug:', ':flag_um:', ':flag_us:', ':flag_uy:', ':flag_uz:', ':flag_va:', ':flag_vc:', ':flag_ve:', ':flag_vg:', ':flag_vi:', ':flag_vn:', ':flag_vu:', ':flag_wf:', ':flag_ws:', ':flag_xk:', ':flag_ye:', ':flag_yt:', ':flag_za:', ':flag_zm:', ':flag_zw:']
        }];

        t.select = function (icon) {
            var u = emojione.shortnameToUnicode(icon);

            var i = t.recentlyUsed.indexOf(u);

            if (i > -1) {
                t.recentlyUsed.splice(i, 1);
            }

            t.recentlyUsed.splice(0, 0, u);

            // Trigger updating the UI
            t.sections[0].emoticons = t.recentlyUsed;

            cookieService.set('emoticons.recentlyUsed', t.recentlyUsed);

            $scope.$emit('emoticonSelected', u);
        }

        var filter = function (array) {
            var seen = {};
            return array.filter(function (item) {
                return seen.hasOwnProperty(item) ? false : (seen[item] = true);
            });
        }

        // INIT
        t.$onInit = function () {
            // Remove duplicates, if any.
            t.recentlyUsed = filter(cookieService.get('emoticons.recentlyUsed', []));

            t.sections[0].emoticons = t.recentlyUsed;
        }
    }
})();