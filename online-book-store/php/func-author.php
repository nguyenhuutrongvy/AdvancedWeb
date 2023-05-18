<?php

# Get all Author function
function get_all_author()
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/authors",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# Get  Author by ID function
function get_author($id)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/authors/" . $id,
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}
