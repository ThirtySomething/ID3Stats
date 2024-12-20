# Tracks artist

Number of total tracks for each artist:

```sql
SELECT
    artist,
    COUNT(artist) AS tracks_total
FROM
    mp3import
GROUP BY
    artist
ORDER BY
    tracks_total DESC,
    artist;
```

**NOTE:** For unique tracks each artist there is another query required!
