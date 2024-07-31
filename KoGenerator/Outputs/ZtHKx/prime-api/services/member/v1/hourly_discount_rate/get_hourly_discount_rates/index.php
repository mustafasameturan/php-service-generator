<?php

//include directory
$inc_dir = "../../../../../_includes/";

//include curl
include $inc_dir."curl.php";

//curl instance
$curl = new curl();

//get api file for production or test
$api_data = $inc_dir.$curl->get_api_data();

//get apikey & apiurl
$params = parse_ini_file($api_data);
$api_key = $params['apiKey_Member'];
$api_url = $params['apiUrl_Member'];

//get data
$data = json_decode(file_get_contents('php://input'),true);
$memberId = $data["memberId"];

//create url
$url = $api_url . '/v1/hourlyDiscountRate/getHourlyDiscountRates/'.$memberId;

$curl->url($url)
    ->set_api_key($api_key)
    ->send();

echo json_encode($curl->result,JSON_UNESCAPED_UNICODE);
