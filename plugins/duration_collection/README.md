# Duration collection

Statistics about the duration over the complete collection:

- Shortest track duration
- Average track duration
- Longest track duration
- Duration over all tracks

```sql
SELECT
    MIN(durationms) AS track_duration_min,
    AVG(durationms) AS track_duration_average,
    MAX(durationms) AS track_duration_max,
    SUM(durationms) AS track_duration_total
FROM
    mp3import;
```
