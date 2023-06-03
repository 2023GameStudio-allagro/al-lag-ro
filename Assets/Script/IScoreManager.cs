public interface IScoreManager
{
	void HitEnemy(int count);
	void HitEnemyPerfect(int count);
	void AttackWrongTime();
	void Miss();
	void GetDamagedByEnemy();
}
