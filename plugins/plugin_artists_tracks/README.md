# Artists tracks

This plugin will list all tracks each artist.

```sql
SELECT
    artist,
    title
FROM
    mp3import
ORDER BY
    artist,
    title,
    id;
```

## Sample output

An example of the output can be viewed [here][app_statistic].

[app_statistic]: ./../../sample/All%20tracks%20per%20artist.html
