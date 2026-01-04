-- Seed Exercises for IronAmbit Database
-- Creates 6 exercises that will be used in the workouts

INSERT INTO Exercises (Name, Description, Category, MuscleGroup, Equipment, Difficulty, CreatedAt, UpdatedAt) VALUES
-- Workout 1 Exercises
('Barbell Bench Press', 'Compound upper body exercise targeting chest, shoulders, and triceps. Lie on bench and press barbell from chest to full extension.', 'Strength', 'Chest', 'Barbell', 'Intermediate', datetime('now'), datetime('now')),
('Incline Dumbbell Press', 'Upper chest development exercise performed on an inclined bench. Press dumbbells from chest level to overhead.', 'Strength', 'Chest', 'Dumbbells', 'Intermediate', datetime('now'), datetime('now')),
('Cable Flyes', 'Isolation exercise for chest development. Use cable machine to bring handles together in a hugging motion.', 'Strength', 'Chest', 'Cable Machine', 'Beginner', datetime('now'), datetime('now')),

-- Workout 2 Exercises
('Barbell Squat', 'Fundamental lower body exercise. Place barbell on upper back and squat down until thighs are parallel to ground.', 'Strength', 'Legs', 'Barbell', 'Intermediate', datetime('now'), datetime('now')),
('Romanian Deadlift', 'Hamstring and glute focused exercise. Keep legs mostly straight while lowering barbell down shins.', 'Strength', 'Legs', 'Barbell', 'Intermediate', datetime('now'), datetime('now')),
('Leg Press', 'Machine-based leg exercise. Push platform away using legs while seated in leg press machine.', 'Strength', 'Legs', 'Machine', 'Beginner', datetime('now'), datetime('now'));

-- Seed Workouts for Mike Brown (UserId 5)
-- Workout 1: Upper Body / Chest Day (2 days ago)
-- Workout 2: Lower Body / Leg Day (yesterday)

INSERT INTO Workouts (UserId, UserName, Date, ExerciseId, ExerciseName, Sets, Reps, Weight, Duration, Notes, CreatedAt, UpdatedAt) VALUES
-- Workout 1: Upper Body / Chest Day (2 days ago)
(5, 'Mike Brown', date('now', '-2 days'), 1, 'Barbell Bench Press', 4, 8, 185.0, 45, 'Felt strong today, increased weight from last session. Good form throughout.', datetime('now', '-2 days'), datetime('now', '-2 days')),
(5, 'Mike Brown', date('now', '-2 days'), 2, 'Incline Dumbbell Press', 3, 10, 65.0, 45, 'Focused on slow negatives, really felt the burn in upper chest.', datetime('now', '-2 days'), datetime('now', '-2 days')),
(5, 'Mike Brown', date('now', '-2 days'), 3, 'Cable Flyes', 3, 12, 35.0, 45, 'Great pump to finish chest workout. Maintained constant tension.', datetime('now', '-2 days'), datetime('now', '-2 days')),

-- Workout 2: Lower Body / Leg Day (yesterday)
(5, 'Mike Brown', date('now', '-1 days'), 4, 'Barbell Squat', 5, 5, 225.0, 50, 'Hit a new PR! Form was solid, depth was good. Legs feeling it.', datetime('now', '-1 days'), datetime('now', '-1 days')),
(5, 'Mike Brown', date('now', '-1 days'), 5, 'Romanian Deadlift', 4, 8, 155.0, 50, 'Great hamstring activation, focused on hip hinge movement.', datetime('now', '-1 days'), datetime('now', '-1 days')),
(5, 'Mike Brown', date('now', '-1 days'), 6, 'Leg Press', 3, 12, 315.0, 50, 'Burnout set to finish leg day. Legs are toast!', datetime('now', '-1 days'), datetime('now', '-1 days'));
