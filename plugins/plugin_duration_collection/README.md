# Duration collection

Statistics about the duration over the complete collection:

- Shortest track duration
- Average track duration
- Longest track duration
- Duration over all tracks

```sql
SELECT
    'track_count_total',
    COUNT(id)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_min',
    MIN(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_average',
    AVG(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_max',
    MAX(durationms)
FROM
    mp3import
UNION ALL
SELECT
    'track_duration_total',
    SUM(durationms)
FROM
    mp3import;
```

## Sample output

An example of the output can be viewed [here][app_statistic].

[app_statistic]: ./../../sample/Collection%20durations.html
