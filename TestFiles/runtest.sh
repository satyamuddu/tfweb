#!/bin/bash
set -e

curl -v -X POST \
"http://localhost:5089/api/RatingProposal/newrealtime" \
-H "accept: */*" \
-H "Content-Type: application/json" \
--data "@one-rating.json"



curl -X POST -F "file=@ECAR_ratings_two_facilities.json" http://localhost:5089/api/ratingproposal/uploadjson

