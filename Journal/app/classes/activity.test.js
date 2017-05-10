describe('activity', function () {

  describe('getOffset', function () {
		it('first test', function () {
			var act = {
				'agentColor' : '#000000',
				'startTime' : '1',
				'endTime' : '2',
				'maxTime' : '3'
			};

			var activity = new Activity(act, 0)
			
			var a = activity.getOffset(7);
			expect(a).toBe(1);
		});	
	});

});