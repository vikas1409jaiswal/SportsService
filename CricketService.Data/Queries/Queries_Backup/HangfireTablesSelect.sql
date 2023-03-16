SELECT id, key, value, expireat
	FROM hangfire.counter;
	
SELECT id, key, field, value, expireat, updatecount
	FROM hangfire.hash;
	
SELECT id, stateid, statename, invocationdata, arguments, createdat, expireat, updatecount
	FROM hangfire.job;

SELECT id, jobid, name, value, updatecount
	FROM hangfire.jobparameter;
	
SELECT id, jobid, queue, fetchedat, updatecount
	FROM hangfire.jobqueue;
	
SELECT id, key, value, expireat, updatecount
	FROM hangfire.list;
	
SELECT resource, updatecount, acquired
	FROM hangfire.lock;

SELECT version
	FROM hangfire.schema;
	
SELECT id, data, lastheartbeat, updatecount
	FROM hangfire.server;
	
SELECT id, key, score, value, expireat, updatecount
	FROM hangfire.set;
	
SELECT id, jobid, name, reason, createdat, data, updatecount
	FROM hangfire.state;